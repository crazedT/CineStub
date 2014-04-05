(function () {
    'use strict';

    var controllerId = 'home';

    angular.module('app').controller(controllerId,
        ['$scope', 'dataService', home]);

    function home($scope, dataService) {
        $scope.currentMovies = [];

        activate();

        function activate() {
            dataService.getCurrentMovies().then(
                function(currentMovies) {
                    $scope.currentMovies = currentMovies;
                },
                function() {});
        }
    }
})();
