(function () {
    'use strict';

    var controllerId = 'topnav';

    angular.module('app').controller(controllerId,
        ['$scope', 'authService', topnav]);

    function topnav($scope, authService) {
        $scope.logout = function() {
            authService.logout()
                .then(function() {}, function() {});
        };

        $scope.authInfo = authService.authInfo;


    }
})();
