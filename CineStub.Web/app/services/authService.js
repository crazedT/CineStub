(function () {
    'use strict';

    var serviceId = 'authService';

    angular.module('app').factory(serviceId, ['$http', '$q', '$cookieStore', authService]);

    function authService($http, $q, $cookieStore) {

        

        // authInfo.AccessLevel
        // 0 - Anonymous, 1 - Authenticated, 2 - Admin

        var authInfo = {
            isAuthenticated: false,
            accessLevel: 0,
            username: ''
        };


        var service = {
            register: register,
            login: login,
            logout: logout,
            authInfo: authInfo
        };

        getUserInfo();
        
        return service;


        function register(registerCredentials) {
            var deferred = $q.defer();

            $http({
                    method: 'POST',
                    url: '/api/account/register',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    data: {
                        UserName: registerCredentials.username,
                        Password: registerCredentials.password,
                        ConfirmPassword: registerCredentials.confirmPassword
                    }
                })
                .success(function(data, status, headers, config) {
                    deferred.resolve();
                })
                .error(function(data, status, headers, config) {
                    var errors = [];
                    _.each(_.values(data.ModelState), function(modelStateArray) {
                        _.each(modelStateArray, function(error) {
                            errors.push(error);
                        });
                    });
                    deferred.reject(errors);
                });

            return deferred.promise;
        }

        function login(loginCredentials) {
            var deferred = $q.defer();

            $http({
                    method: 'POST',
                    url: '/token',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    data: 'grant_type=password&username=' + loginCredentials.username + '&password=' + loginCredentials.password

                })
                .success(function(data, status, headers, config) {
                    $cookieStore.put('token', data.access_token);
                    getUserInfo();
                    deferred.resolve();
                })
                .error(function(data, status, headers, config) {
                    deferred.reject(data.error_description);
                });

            return deferred.promise;
        }

        function logout() {
            var deferred = $q.defer();

            $http({
                    method: 'POST',
                    url: '/api/account/logout',
                    headers: {
                        Authorization: 'Bearer ' + $cookieStore.get('token')
                    }
                })
                .success(function (data, status, headers, config) {
                    $cookieStore.remove('token');
                    setAuthInfo();
                })
                .error(function(data, status, headers, config) {
                });

            return deferred.promise;
        }

        function getUserInfo() {
            var deferred = $q.defer();

            if (!$cookieStore.get('token')) {
                deferred.reject();
            } else {
                $http({
                        method: 'GET',
                        url: '/api/account/userinfo',
                        headers: {
                            Authorization: 'Bearer ' + $cookieStore.get('token')
                        }
                    })
                    .success(function(data, status, headers, config) {
                        setAuthInfo(data);
                        deferred.resolve(data);
                    })
                    .error(function(data, status, headers, config) {
                        setAuthInfo();
                        deferred.reject();
                    });
            }

            return deferred.promise;
        }

        function setAuthInfo(data) {
            if (!data) {
                resetAuthInfo();
            } else {
                authInfo.isAuthenticated = true;
                authInfo.accessLevel = data.AccessLevel;
                authInfo.username = data.UserName;
            }

            function resetAuthInfo() {
                authInfo.isAuthenticated = false;
                authInfo.accessLevel = 0;
                authInfo.username = '';
            }
        }
    }

})();