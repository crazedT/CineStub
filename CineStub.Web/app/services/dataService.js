(function () {
    'use strict';

    var serviceId = 'dataService';

    // TODO: replace app with your module name
    angular.module('app').factory(serviceId, ['$http', '$q', '$cookieStore', dataService]);

    function dataService($http, $q, $cookieStore) {
        var service = {
            getMovie: getMovie,
            getMovies: getMovies,
            getCurrentMovies: getCurrentMovies,
            getUpcomingMovies: getUpcomingMovies,
            getMovieShowtimes: getMovieShowtimes,
            addMovie: addMovie,
            updateMovie: updateMovie,
            searchTmdb: searchTmdb,
            getSchedules: getSchedules,
            addSchedule: addSchedule,
            getSlotsForSchedule: getSlotsForSchedule,
            scheduleMovie: scheduleMovie,
            unscheduleMovie: unscheduleMovie
        };

        return service;

        function getMovie(movieId) {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: '/api/movie/' + movieId
                })
                .success(function(movie) {
                    deferred.resolve(movie);
                })
                .error(function () {
                console.log('reject');
                    deferred.reject();
                });

            return deferred.promise;
        }

        function getMovies() {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: '/api/movie/'
                })
                .success(function(movies) {
                    deferred.resolve(movies);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function getCurrentMovies() {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: 'api/movies/current'
            }).success(function(currentMovies) {
                deferred.resolve(currentMovies);
            }).error(function() {
                deferred.reject();
            });

            return deferred.promise;
        }

        function getUpcomingMovies() {

        }

        function getMovieShowtimes(movieId) {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: 'api/movie/showtimes/' + movieId
                })
                .success(function(slotGroups) {
                    deferred.resolve(slotGroups);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function searchTmdb(title) {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: 'api/movie/searchtmdb/' + title,
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function(searchResults) {
                    deferred.resolve(searchResults);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function addMovie(tmdbId) {
            var deferred = $q.defer();

            $http({
                    method: 'POST',
                    url: 'api/movie/',
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    },
                    data: tmdbId
                })
                .success(function() {
                    deferred.resolve();
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function getSchedules() {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: 'api/schedule',
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function(schedules) {
                    deferred.resolve(schedules);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function addSchedule() {
            var deferred = $q.defer();

            $http({
                    method: 'POST',
                    url: 'api/schedule/addschedule',
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function(schedule) {
                    deferred.resolve(schedule);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function getSlotsForSchedule(scheduleId) {
            var deferred = $q.defer();

            $http({
                    method: 'GET',
                    url: 'api/schedule/SlotsGroupedByDate/' + scheduleId,
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function(slotGroups) {
                    deferred.resolve(slotGroups);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function scheduleMovie(slotId, movieId) {
            var deferred = $q.defer();

            $http({
                method: 'PUT',
                url: 'api/Schedule/ScheduleMovie/',
                headers: {
                    Authorization: 'Bearer ' + $cookieStore.get('token')
                },
                data: {
                    slotId: slotId,
                    movieId: movieId
                }
            })
                .success(function (slots) {
                    deferred.resolve(slots);
                })
                .error(function () {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function unscheduleMovie(rootSlotId) {
            var deferred = $q.defer();

            $http({
                    method: 'PUT',
                    url: 'api/schedule/unscheduleMovie/' + rootSlotId,
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function() {
                    deferred.resolve();
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function updateMovie(movie) {
            var deferred = $q.defer();

            $http({
                    method: 'PUT',
                    url: 'api/movie/' + movie.movieId,
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    },
                    data: movie
                })
                .success(function() {
                    deferred.resolve();
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }
    }
})();