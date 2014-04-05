(function () {
    'use strict';

    var controllerId = 'movie_details';

    // TODO: replace app with your module name
    angular.module('app').controller(controllerId,
        ['$scope', 'resolvedMovie', 'showtimes', movieDetails]);

    function movieDetails($scope, resolvedMovie, showtimes) {
        $scope.movie = resolvedMovie;
        $scope.showtimes = showtimes;
        $scope.selectedSlotGroup = {};

        $scope.selectSlotGroup = function(slotGroup) {
            $scope.selectedSlotGroup = slotGroup;
        };

        $scope.slotGroupSelected = function(slotGroup) {
            return $scope.selectedSlotGroup == slotGroup;
        };


        $scope.carouselInterval = 10000;

        $scope.jwOptions = {
            file: resolvedMovie.youtubeTrailerUrl,
            width: "80%",
            aspectratio: "16:9"
        };

    }
})();
