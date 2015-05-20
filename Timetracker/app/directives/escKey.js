angular.module("timeTracker").directive('escKey', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, controller) {
            element.on('keydown', function(event) {
                if (event.which === 27) {
                    scope.$apply(function () {
                        scope.$eval(attrs.escKey);
                    });

                    event.preventDefault();
                }
            });
        }
    }
});

angular.module("timeTracker").directive('enterKey', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs, controller) {
            element.on('keydown', function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.enterKey);
                    });

                    event.preventDefault();
                }
            });
        }
    }
});