angular.module("timeTracker").controller("ProjectController",
    function ($scope, projectService, projectCommandService, $interval) {
        //var today = new Date();
        //var oneWeekAgo = new Date();
        //oneWeekAgo.setDate(oneWeekAgo.getDate() - 7);
        var timer;
        var refreshInterval=60000;
        //$scope.projects = projectService.getProjects();
        var getTime = function (prj) {
            var prjCommand = { projectId: prj.project.projectId, action: "time", actionDetails: "" };
            $scope.elapsedTime = projectCommandService.process(prjCommand);
        }

        $scope.projects = projectService.getProjects().$promise.then(function (data) {
            $scope.projects = data;
            angular.forEach($scope.projects, function (item) {
                if (item.hasActiveLog) {
                    getTime(item);
                    timer = $interval(function () {
                        getTime(item);
                    }, refreshInterval);
                }
            });
        });



        $scope.start = function (p) {
            var prjCommand = { projectId: p.project.projectId, action: "start", actionDetails: "" };
            angular.forEach($scope.projects, function (item) { item.hasActiveLog = false; });
            p.hasActiveLog = true;
            projectCommandService.process(prjCommand);

            timer = $interval(function () {
                getTime(item);
            }, refreshInterval);
        }
        $scope.stop = function (p) {
            var prjCommand = {
                projectId: p.project.projectId,
                action: "stop",
                actionDetails: ""
            };
            p.hasActiveLog = false;
            projectCommandService.process(prjCommand);

            if (angular.isDefined(timer)) {
                $interval.cancel(timer);
                timer = undefined;
            }
        }


    });
