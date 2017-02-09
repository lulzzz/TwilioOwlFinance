/* cases.service.js */

(function (angular, app) {
    function factory($resource, Api) {
        return $resource(Api.weather.url, null,
            {
                query: {
                    url: Api.weather.url,
                    method: 'GET',
                    isArray: false,
                    transformResponse: Api.defaultResponseTransform
                }
            }
        );
    }

    factory.$inject = ['$resource', 'Api'];
    app.factory('WeatherService', factory);
})(angular, app);
