(function () {
    'use strict';

    var controllerId = 'movie_add';

    angular.module('app').controller(controllerId,
        ['$scope', '$location', 'dataService', movieAdd]);

    function movieAdd($scope, $location, dataService) {
        $scope.movie = {};
        $scope.searchResults = [];

        $scope.searchTmdb = function (title) {
            dataService.searchTmdb(title)
                .then(function (searchResults) {
                    $scope.searchResults = searchResults;
                });
        };

        $scope.addMovie = function(searchResult) {
            dataService.addMovie(searchResult.tmdbId)
                .then(
                function () {
                },
                function() {
                    
                });
        };
    }
})();
