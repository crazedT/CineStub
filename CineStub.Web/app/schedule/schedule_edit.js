(function () {
    'use strict';

    var controllerId = 'schedule_edit';

    // TODO: replace app with your module name
    angular.module('app').controller(controllerId,
        ['$scope', 'dataService', scheduleEdit]);

    function scheduleEdit($scope, dataService) {
        $scope.schedules = [];
        $scope.movies = [];
        $scope.slotGroups = [];
        $scope.selectedSchedule = {};
        $scope.selectedMovie = null;

        $scope.addSchedule = function() {
            dataService.addSchedule()
                .then(function(schedule) {
                    $scope.schedules.push(schedule);
                }, function() {});
        };

        $scope.selectSchedule = function(schedule) {
            dataService.getSlotsForSchedule(schedule.id)
                .then(
                    function (slotGroups) {
                        $scope.slotGroups = slotGroups;
                        $scope.selectedSchedule = schedule;
                    },
                    function() {});
        };

        $scope.scheduleSelected = function(schedule) {
            return $scope.selectedSchedule == schedule;
        };

        $scope.selectMovie = function(movie) {
            $scope.selectedMovie = movie;
        };

        $scope.movieSelected = function(movie) {
            return $scope.selectedMovie == movie;
        };

        $scope.anyMovieSelected = function () {

            if ($scope.selectedMovie == null) {
                return false;
            }
            return true;
        };

        $scope.slotIsOpen = function(slot) {
            return slot.isOpen;
        };

        $scope.isRoot = function(slot) {
            return slot.isRoot;
        };

        $scope.scheduleMovie = function (slot) {
            dataService.scheduleMovie(slot.slotId, $scope.selectedMovie.id)
                .then(
                    function() {
                        $scope.selectSchedule($scope.selectedSchedule);
                    },
                    function() {

                    });
        };

        $scope.unscheduleMovie = function (slot) {
            dataService.unscheduleMovie(slot.slotId)
                .then(
                    function () {
                        $scope.selectSchedule($scope.selectedSchedule);
                    },
                    function () {

                    });
        };


        activate();

        function activate() {
            dataService.getSchedules()
                .then(function(schedules) {
                    $scope.schedules = schedules;
                }, function() {
                });

            dataService.getMovies()
                .then(
                    function(movies) {
                        $scope.movies = movies;
                    },
                    function() {
                    });
        }
    }
})();
