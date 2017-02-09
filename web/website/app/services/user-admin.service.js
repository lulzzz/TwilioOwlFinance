/* user-admin.service.js */

(function (angular, app) {
    'use strict';

    app.factory('userAdminService', factory);

    factory.$inject = ['$rootScope', 'OwlFinanceServiceBase'];
    function factory($rootScope, OwlFinanceServiceBase) {
        function UserAdminService() {
            OwlFinanceServiceBase.call(this);
        }

        UserAdminService.prototype = Object.create(OwlFinanceServiceBase.prototype);
        UserAdminService.prototype.constructor = UserAdminService;

        UserAdminService.prototype.getUser = function (id) {
            return this.get(`${this.api.admin.users.url}/${id}`);
        }

        UserAdminService.prototype.getUsers = function () {
            return this.get(this.api.admin.users.url);
        }

        UserAdminService.prototype.getCustomers = function () {
            return this.get(this.api.admin.users.customers.url);
        }

        UserAdminService.prototype.saveUser = function (user) {
            return this.post(this.api.admin.users.url, user);
        }

        UserAdminService.prototype.deleteUser = function (user) {
            return this.delete(`${this.api.admin.users.url}/${user.id}`, user);
        }

        return new UserAdminService();
    }
})(angular, app);
