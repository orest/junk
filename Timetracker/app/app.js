"use strict";

angular.module("timeTracker", ['ngResource', 'ngRoute', 'ngCookies', 'ngTable',
    'angular-loading-bar','ngAnimate', 'ui.bootstrap.datetimepicker', 'ui.sortable'])
    .config(function ($routeProvider, $locationProvider, cfpLoadingBarProvider) {
        $routeProvider.when('/tracker', { templateUrl: '/app/partials/entryManager.html', controller: 'JogTrackerController' });
        $routeProvider.when('/reports', { templateUrl: '/app/partials/reports.html', controller: 'ReportsController' });
        $routeProvider.when('/login', { templateUrl: '/app/partials/login.html', controller: 'LoginController' });
        $routeProvider.when('/register', { templateUrl: '/app/partials/register.html', controller: 'EditProfileController' });
        $routeProvider.when('/editProfile', { templateUrl: '/app/partials/editProfile.html', controller: 'EditProfileController' });
        $routeProvider.when('/viewProfile/:userName', { templateUrl: '/app/partials/viewProfile.html', controller: 'ViewProfileController' });
        $routeProvider.otherwise({ redirectTo: '/tracker' });
        $locationProvider.html5Mode(true);
        cfpLoadingBarProvider.includeSpinner = false;
    });

angular.module("timeTracker").factory('moment', function () {
    return window.moment;
});

angular.module("timeTracker").factory('_', function () {
    return window._;
});
