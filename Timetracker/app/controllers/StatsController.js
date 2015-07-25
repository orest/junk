angular.module("timeTracker").controller("StatsController",
    function ($scope, projectService, projectCommandService, $interval, globals, models, _, statsService) {
        $scope.reportTitle = "This Week";
        $scope.filter="";
        function refreshStats(filter) {
            $scope.weekTotalHours = 0;
            $scope.weekTotalAmount = 0;

            statsService.get('weeklyreport', filter).$promise.then(function (data) {
                $scope.stat = data;
                _.each($scope.stat, function (item) {
                    $scope.weekTotalHours += Number(item.elapsed);
                    $scope.weekTotalAmount += item.total;
                });
            });
        }


        $scope.setFilter = function (data) {
            $scope.filter = data;
            console.log(data);
            if(data==="today")
                $scope.reportTitle = "Today";
            else if (data === "month")
                $scope.reportTitle = "This month";
            else if (data === "last")
                $scope.reportTitle = "Last month";
            else
                $scope.reportTitle = "This week";

            refreshStats(data);
        };
        
        refreshStats('week');
    });
