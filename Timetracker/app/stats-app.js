"use strict";

angular.module("timeTracker", ['ngResource', 'ngRoute', 'ngCookies', 'ngTable',
    'angular-loading-bar', 'ngAnimate', 'ui.bootstrap', 'ui.sortable'])
    .config(function ($routeProvider, $locationProvider, cfpLoadingBarProvider) {
    
    });

angular.module("timeTracker").factory('moment', function () {
    return window.moment;
});

angular.module("timeTracker").factory('_', function () {
    return window._;
});
