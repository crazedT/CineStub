(function () {
    'use strict';

    var controllerId = 'movie_edit';

    angular.module('app').controller(controllerId,
        ['$scope', 'dataService', 'resolvedMovie', movieEdit]);

    function movieEdit($scope, dataService, resolvedMovie) {
        $scope.movie = resolvedMovie;

        $scope.saveMovie = function() {
            dataService.updateMovie($scope.movie)
                .then(function() {}, function() {});
        };
    }
})();
