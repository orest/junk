angular.module("timeTracker").controller("ProjectController",
    function ($scope, projectService, projectCommandService, todoService, $interval, globals, models) {
        var timer;
        $scope.projects = [];
        $scope.stopMessage = "";
        var t = new models.Task();

        $scope.taskStatuses = t.statuses;
        $scope.todos = todoService.getAllTodos();

        var getTime = function (prj) {
            var prjCommand = { projectId: prj.project.projectId, action: "time", actionDetails: "" };
            $scope.elapsedTime = projectCommandService.process(prjCommand);
        },

        cancelTimer = function () {
            if (angular.isDefined(timer)) {
                $interval.cancel(timer);
                timer = undefined;
            }
        };

        //refresh projects
        function refreshProjects() {
            projectService.getProjects().$promise.then(function (data) {
                $scope.projects = data;
                angular.forEach($scope.projects, function (item) {
                    if (item.hasActiveLog) {
                        getTime(item);
                        timer = $interval(function () {
                            getTime(item);
                        }, globals.refreshInterval);
                    }
                });
            });

        }

        //Start buttons
        $scope.start = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "start");
            angular.forEach($scope.projects, function (item) { item.hasActiveLog = false; });
            p.hasActiveLog = true;
            projectCommandService.process(prjCommand);
            $scope.elapsedTime = { minutes: 0, hours: 0 };

            timer = $interval(function () {
                getTime(p);
            }, globals.refreshInterval);
        }
        //Restart
        $scope.restart = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "restart");
            projectCommandService.process(prjCommand);
            p.isPaused = false;
            timer = $interval(function () {
                getTime(p);
            }, globals.refreshInterval);
        }

        // Stop buttons
        $scope.stop = function (p) {
            var details = { message: $scope.stopMessage, time: $scope.elapsedTime.minutes };

            var prjCommand = new models.ProjectCommand(p.project.projectId, "stop", JSON.stringify(details));
            p.hasActiveLog = false;
            p.stopping = false;
            $scope.stopMessage = "";
            projectCommandService.process(prjCommand);
            cancelTimer();
        }

        //Pause buttons
        $scope.pause = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "pause");
            p.isPaused = true;
            projectCommandService.process(prjCommand);
            cancelTimer();
        }

        //add task
        $scope.addTask = function (p) {
            var task = new models.Task(0, p.newTaskName, p.project.projectId);
            projectService.addTask(task).$promise.then(function () {
                refreshProjects();
            });
            p.newTaskName = "";
        }

        //add task
        $scope.updateTask = function (task) {
            projectService.saveTask(task);
            task.editing = false;
        }

        //add todos
        $scope.addTodo = function () {
            var todo = new models.Todo(0, $scope.newTodo);
            todoService.addTodo(todo);
            $scope.todos.push(todo);
            $scope.newTodo = "";
        }


        refreshProjects();
       
    });
