angular.module("timeTracker").controller("StatsController",
    function ($scope, projectService, projectCommandService, $interval, globals, models, _, statsService) {
        $scope.reportTitle = "This Week";
        $scope.filter = "";
        $scope.startDate = new Date();
        $scope.endDate = new Date();
        

        function refreshStats(filter) {
            $scope.weekTotalHours = 0;
            $scope.weekTotalAmount = 0;
            if (filter === "custom-date") {
                statsService.getByDate($scope.startDate, $scope.endDate).$promise.then(function (data) {
                    $scope.stat = data;
                    _.each($scope.stat, function (item) {
                        $scope.weekTotalHours += Number(item.elapsed);
                        $scope.weekTotalAmount += item.total;
                    });
                });
            } else {
                statsService.get('weeklyreport', filter).$promise.then(function (data) {
                    $scope.stat = data;
                    _.each($scope.stat, function (item) {
                        $scope.weekTotalHours += Number(item.elapsed);
                        $scope.weekTotalAmount += item.total;
                    });
                });
            }



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
                    series: [{ name: 'hours', showInLegend: false, data: hours }]
                });

            });

        }


        $scope.setFilter = function (data) {
            $scope.filter = data;
            console.log(data);
            if (data === "today")
                $scope.reportTitle = "Today";
            else if (data === "month")
                $scope.reportTitle = "This month";
            else if (data === "last")
                $scope.reportTitle = "Last month";
            else
                $scope.reportTitle = "This week";

            refreshStats(data);
        };


        
        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };

        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };

       

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.formats = ['MM/dd/yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];

        // var weekday = moment("12-25-1995", "MM-DD-YYYY").week();
        $scope.popup1 = {
            opened: false
        };

        $scope.popup2 = {
            opened: false
        };

        $scope.startDateChange=function ()
        {
            
            $scope.endDate = moment($scope.startDate).add(6, 'day').toDate();
        }

        refreshStats('week');
    });
