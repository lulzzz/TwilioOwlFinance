/* case-details.controller.js */
(function (angular, app) {
    'use strict';

    app.controller('caseDetailsController', controller);

    controller.$inject = ['$scope', '$routeParams', '$timeout', 'auth', 'store', 'signalEvents', 'CaseNavigationService', 'twilioIntegration', 'AgentService', 'CaseService', 'WeatherService'];
    function controller($scope, $routeParams, $timeout, auth, store, signalEvents, nav, twilio, agentService, caseService, weatherService) {
        var caseId, chatChannelName, audioChannelName, ipMessagingClient, videoClient, activeRoom, localMedia, voiceCallResponse;

        caseId = $routeParams.id;
        chatChannelName = `case${caseId}`;

        // ui state
        $scope.isCallMode = false;
        $scope.timerRunning = false;
        $scope.isAskingForCall = false;
        $scope.hangUpButtonDisabled = true;
        $scope.pairedCustomer = {};
        $scope.auth = auth;
        $scope.messageHistory = [];

        // event handlers
        $scope.sendMessage = sendMessage;
        $scope.closeCase = closeCase;
        $scope.sendDocuSignMessage = sendDocuSignMessage;
        $scope.openDocuSignDocument = openDocuSignDocument;
        $scope.hangup = hangup;
        $scope.requestCall = requestCall;
        $scope.startPSTNCall = startPTSNCall;
        $scope.acceptCall = acceptCall;
        $scope.declineCall = declineCall;

        // unload
        $scope.$on("$destroy", function () {
            disconnectAll();
        });

        caseService.get({ id: caseId }, function (currentCase) {
            $scope.currentCase = currentCase;

            audioChannelName = "account" + currentCase.accountId;
            initMobileChannel();

            nav.select(currentCase);

            caseService.getAccountCases({ id: currentCase.accountId }, function (openCases) {
                $scope.openCases = openCases;
            });

            caseService.getAccountHistory({ id: currentCase.accountId }, function (events) {
                $scope.accountHistory = events;
            });

            caseService.getAccountStatements({ id: currentCase.accountId }, function (statements) {
                $scope.statements = statements;
            });

            caseService.getAccountTransactions({ id: currentCase.accountId }, function (transactions) {
                $scope.transactions = transactions;
            });

            weatherService.query({ customerID: currentCase.customerId }, function (weather) {
                $scope.weather = weather;
            });

            agentService.getPairedCustomer().then(function (customer) {
                $scope.pairedCustomer = customer;
            });
        });

        (function () {
            // init chat messaging
            twilio.getMessagingToken({ device: 'browser', identityId: '', name: '', picture: '' })
                .then(function (response) {
                    var am = new Twilio.AccessManager(response.token);
                    ipMessagingClient = new Twilio.Chat.Client(response.token);

                    ipMessagingClient.initialize()
                        .then(function() {
                            ipMessagingClient
                                .getChannelByUniqueName(chatChannelName)
                                .then(joinMessagingChannel)
                                .catch(handleMessagingErrors);
                        });
                });

            // init programmable video
            twilio.getConversationsToken({ name: 'browser' })
                .then(function (response) {
                    var am = new Twilio.AccessManager(response.token);
                    videoClient = new Twilio.Video.Client(response.token);
                });

            // init ptsn voice calls
            twilio.getVoiceCallToken({ name: 'browser' })
                .then(function (response) {
                    voiceCallResponse = response;
                    Twilio.Device.setup(voiceCallResponse.token);
                    Twilio.Device.disconnect(function () {
                        resetCallState();
                    });
                });
        })();

        function startPTSNCall($event) {
            $event.preventDefault();

            $scope.isCallMode = true;
            $scope.hangUpButtonDisabled = true;
            $scope.$broadcast('timer-start');
            $scope.timerRunning = true;

            $timeout(function () {
                $scope.hangUpButtonDisabled = false;
            }, 3000);

            Twilio.Device.connect({ 'PhoneNumber': voiceCallResponse.phoneNumber });
        }

       function initMobileChannel() {
           signalEvents.client(function (client) {
               client.MessageReceived = function (id, message) {
                   if (message === "incomingcall") {
                       $scope.$evalAsync(function () {
                           $scope.isAskingForCall = true;
                       });
                   } else {
                       clientMessageReceived(message);
                   }
               };
           });

           signalEvents.server.subscribe($scope.currentCase.accountId);
       }

       function call() {
           localMedia = new Twilio.Video.LocalMedia();

           // only add microphone
           localMedia.addMicrophone().then(function() {
               videoClient.connect({
                   to: audioChannelName,
                   localMedia: localMedia
               }).then(function (room) {
                   initRoom(room);
                   callStarted();
                   $scope.$apply();
               }, function (error) {
                   console.error('Failed to connect to the room.', error);
               });
           });
        }

        function requestCall($event) {
            $event.preventDefault();
            requestCallWithCustomer();
            call();
        }

        function acceptCall($event) {
            $event.preventDefault();
            $scope.isAskingForCall = false;
            call();
        }

        function declineCall($event) {
            $event.preventDefault();
            $scope.isAskingForCall = false;
        }

        function callStarted() {
            $scope.hangUpButtonDisabled = false;
            $scope.isCallMode = true;
            $scope.$broadcast('timer-start');
            $scope.timerRunning = true;
        }

        function requestCallWithCustomer() {
            signalEvents.server.send($scope.currentCase.accountId, "incomingcall");
        }

        function hangup($event) {
            $event.preventDefault();

            if ($scope.hangUpButtonDisabled) {
                alert('Please wait a few moments before disconnecting.');
                return;
            }

            disconnectAll();
        }

        function disconnectAll() {
            if (Twilio.Device !== null) {
                Twilio.Device.disconnectAll();
            }

            if (localMedia) {
                localMedia.stop();
                localMedia = null;
            }

            if (activeRoom) {
                activeRoom.disconnect();
                activeRoom = null;
            }
        }

        function clientMessageReceived(message) {
            $scope.$evalAsync(function () {
                var isDocuSign = message === 'SIGNED';

                if (message) {
                    if (message.startsWith('docusign_')) {
                        message = "The agent sent a request for signature.";
                    }

                    $scope.messageHistory.push({
                        author: { picture: 'images/info.png' },
                        body: isDocuSign ? null : message,
                        sent: new Date().toLocaleString('en-US', { day: '2-digit', month: 'short', hour: '2-digit', minute: '2-digit' }),
                        isDocusign: isDocuSign
                    });

                    $timeout(function () {
                        var element = $('.message-wrapper');
                        element.scrollTop(element.prop('scrollHeight'));
                    }, 500);
                }
            });
        }

        function initRoom(room) {
            activeRoom = room;
            console.log('Joined audio room.');

            // attach local media
            room.localParticipant.media.attach("#local-media");

            // attach media for existing participants
            room.participants.forEach(function (participant) {
                participant.media.attach("#remote-media");
                clientMessageReceived(participant.identity + ' is already in the room.');
            });

            // when a participant connects
            room.on('participantConnected', function (participant) {
                participant.media.attach("#remote-media");
                clientMessageReceived($scope.currentCase.accountOwner + ' is connected with an agent.');
                console.log("Participant '" + participant.identity + "' connected");
            });

            // when a participant disconnects
            room.on('participantDisconnected', function (participant) {
                participant.media.detach();
                clientMessageReceived($scope.currentCase.accountOwner + ' spoke with an agent.');

                // hangup when no one else is in the room
                if (room.participants == null || room.participants.size === 0) {
                    disconnectAll();
                    resetCallState();
                }

                console.log("Participant '" + participant.identity + "' disconnected");
            });

            // when call ends
            room.on('disconnected', function () {
                room.localParticipant.media.detach();
                room.participants.forEach(function (participant) {
                    participant.media.detach();
                });

                disconnectAll();
                resetCallState();
                console.log('Disconnected from room.');
            });
        }

        function resetCallState() {
            $scope.isCallMode = false;
            $scope.$broadcast('timer-stop');
            $scope.timerRunning = false;

            $timeout(function () {
                $scope.$apply();
            });
        }

        function joinMessagingChannel(channel) {
            if (channel) {
                $scope.channel = channel;
                channel.join().then(joinedMessagingChannel);
                channel.on('messageAdded', addMessages);
            }
        }

        function handleMessagingErrors(error) {
            switch (error.body.status) {
                case 404:
                    createMessagingChannel();
                    break;
                case 403:
                    console.error('User forbidden to join chat channel.');
                    break;
            }
        }

        function createMessagingChannel() {
            ipMessagingClient.createChannel({
                uniqueName: chatChannelName,
                friendlyName: `Case #${caseId}`
            }).then(joinMessagingChannel);
        }

        function joinedMessagingChannel(channel) {
            // default value retrieves last 30 messages
            channel.getMessages()
                .then(function(messages) {
                    var profile = store.get('profile');

                    var author = JSON.stringify({
                        identityId: profile.user_id,
                        name: profile.name,
                        picture: profile.picture
                    });

                    var joinMessage = {
                        author: author,
                        body: `Case #${$scope.currentCase.id} created by ${$scope.currentCase.accountOwner}.`,
                        timestamp: new Date()
                    };

                    if (messages.items) {
                        messages.items.unshift(joinMessage);
                    }

                    addMessages(messages);
                });
        }

        function addMessages(page) {
            var messages;

            if (page.items) {
                messages = page.items;
            } else {
                messages = [page];
            }

            $scope.$evalAsync(function () {
                if (messages) {
                    for (var i = 0; i < messages.length; i++) {
                        var isDocusign = messages[i].body === 'SIGNED';

                        $scope.messageHistory.push({
                            author: JSON.parse(messages[i].author),
                            body: isDocusign ? null : messages[i].body,
                            sent: messages[i].timestamp,
                            isDocusign: isDocusign
                        });
                    }
                }
       
                $timeout(function () {
                    var element = $('.message-wrapper');
                    element.scrollTop(element.prop('scrollHeight'));
                }, 500);
            });
        }

        function sendDocuSignMessage() {
            agentService.sendDocuSignDocument(caseId)
                .then(function (response) {
                    var signMessage = "docusign_" + response.signUrl;
                    signalEvents.server.send($scope.currentCase.accountId, signMessage);
                });
        }

        function openDocuSignDocument($event) {
            $event.preventDefault();
            agentService.getDocument(caseId)
                .then(function (response) {
                    window.open(response.documentUrl);
                });
        }

        function closeCase(caseId) {
            agentService.closeCase(caseId)
                .then(function (response) {
                    if (response.isSuccess) {
                        signalEvents.server.send($scope.currentCase.accountId, 'closecase');
                        nav.close($scope.currentCase);
                    }
                });
        }

        function sendMessage() {
            if ($scope.message) {
                $scope.channel.sendMessage($scope.message);
            }
            $scope.message = null;
        }
    };
})(angular, app);