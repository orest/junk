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


            statsService.get('week-series').$promise.then(function (data) {
                var weeks = [];
                var hours = [];
                _.each(data, function (item) {                   
                    weeks.push(item.weekId);
                    hours.push(Number(item.elapsedHours));
                });

                $('#weeklyReport').highcharts({
                    chart: {
                        type: 'column',
                        //margin: 20,
                        //options3d: {
                        //    enabled: false,
                        //    alpha: 10,
                        //    beta: 25,
                        //    depth: 70
                        //}
                    },
                    plotOptions: {
                        column: {
                            dataLabels: {
                                enabled: true
                            },
                            enableMouseTracking: false
                        }
                    },
                    title: { text: null },
                    xAxis: { categories: weeks },
                    yAxis: { title: { text: 'Hours' } },
                    tooltip: { valueSuffix: ' hours' },
                    legend: { layout: 'vertical', align: 'right', verticalAlign: 'middle', borderWidth: 0 },
                    series: [{ name:'hours',  showInLegend: false,    data: hours }]
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
