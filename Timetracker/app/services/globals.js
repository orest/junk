angular.module("timeTracker").factory('globals', function () {

    var urls = {
        apiBase: 'http://localhost:6709/api/'
    };
    var refreshInterval = 60000;
    return {
        urls: urls,
        refreshInterval: refreshInterval
    }
});