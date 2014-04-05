(function () {
    'use strict';

    var controllerId = 'login';

    angular.module('app').controller(controllerId,
        ['$scope', 'authService', login]);

    function login($scope, authService) {
        $scope.loginCredentials = {
            username: '',
            password: ''
        };

        $scope.error = '';

        $scope.login = function() {
            authService.login($scope.loginCredentials)
                .then(
                    function() {

                    },
                    function (error) {
                        $scope.error = error;
                    });
        };
    }
})();
