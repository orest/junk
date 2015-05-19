angular.module("timeTracker").factory('globals', function () {

    var urls = {
        apiBase: 'api/'
    };
    var refreshInterval = 60000;
    return {
        urls: urls,
        refreshInterval: refreshInterval
    }
});