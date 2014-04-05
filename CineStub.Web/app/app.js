(function () {
    'use strict';

    var id = 'app';

    // TODO: Inject modules as needed.
    var app = angular.module('app', [
        'ngCookies',
        'ui.router',
        'ui.router.stateHelper',
        'angularMoment',
        'ui.bootstrap',
        'angular-jwplayer'
    ]);

    app.config(['$stateProvider', '$urlRouterProvider', 'stateHelperProvider', function ($stateProvider, $urlRouterProvider, stateHelperProvider) {
        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: 'app/home/home.html',
                controller: 'home'
            })
            .state('register', {
                url: '/register',
                templateUrl: 'app/user/register.html',
                controller: 'register'
            })
            .state('login', {
                url: '/login',
                templateUrl: 'app/user/login.html',
                controller: 'login'
            })
            .state('addMovie', {
                url: '/movie/add',
                templateUrl: 'app/movie/add.html',
                controller: 'movie_add'
            })
            .state('movie', {
                url: '/movie/:movieId',
                templateUrl: 'app/movie/details.html',
                controller: 'movie_details',
                resolve: {
                    resolvedMovie: resolveMovie,
                    showtimes: getMovieShowtimes
                }
            })
            .state('schedule', {
                url: '/schedule',
                template: '<div ui-view></div>'
            })
            .state('schedule.edit', {
                url: '/edit',
                templateUrl: 'app/schedule/edit.html',
                controller: 'schedule_edit'
            });

        function resolveMovie($stateParams, $q, dataService) {
            var deferred = $q.defer();
            dataService.getMovie($stateParams.movieId)
                .then(function (movie) {
                    deferred.resolve(movie);
                }, function () {
                    deferred.reject();
                });
            return deferred.promise;
        }

        function getMovieShowtimes($stateParams, $q, dataService) {
            var deferred = $q.defer();
            dataService.getMovieShowtimes($stateParams.movieId)
                .then(function (slotGroups) {
                    deferred.resolve(slotGroups);
                }, function () {
                    deferred.reject();
                });
            return deferred.promise;
        }
    }]);


    // Execute bootstrapping code and any dependencies.
    // TODO: inject services as needed.
    app.run(['$q', '$rootScope', 'authService',
        function ($q, $rootScope, authService) {
            
        }]);
})();