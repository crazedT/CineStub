(function () {
    'use strict';

    var controllerId = 'register';

    angular.module('app').controller(controllerId,
        ['$scope', 'authService', register]);

    function register($scope, authService) {
        $scope.registerCredentials = {
            username: '',
            password: '',
            confirmPassword: ''
        };

        $scope.errors = [];

        $scope.register = function() {
            authService.register($scope.registerCredentials)
                .then(
                    function() {
                        //todo: route to home or previous page
                    },
                    function(errors) {
                        $scope.errors = errors;
                    });
        };
    }
})();
