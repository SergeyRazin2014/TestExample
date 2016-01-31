/// <reference path="../ArrayHelper.js" />
/// <reference path="../../Scripts/angular.js" />

angular.module("UserRegistrationApp")
    .constant("baseUrl", "/api/PersonRegistration")
    .controller("UserRegistrationCtrl", function ($scope, $http, baseUrl) {
        //получение всех данных из модели
        $scope.refresh = function () {
            $http.get(baseUrl).success(function (PersonAndRoles) {
                $scope.persons = PersonAndRoles.AllPersons;
                $scope.roles = PersonAndRoles.AllRoles;

                $scope.FullName = "";
                $scope.Login = "";
                $scope.Password = "";
            });
        }

        // создание нового элемента
        $scope.create = function () {
            var newPers = {};

            newPers.Id = 0;

            newPers.FullName = $scope.FullName;
            newPers.Login = $scope.Login;
            newPers.Password = $scope.Password;

            newPers.Roles = $scope.roles.filter(function (rol) {
                return rol.isActive;
            });

            if (!$scope.isValid(newPers))
                return;

            if (newPers.FullName && newPers.Login && newPers.Password) {
                $http.post(baseUrl, newPers).success(function (addedPerson) {
                    $scope.persons.push(addedPerson);
                });
            }

            $scope.refresh();
        }

        // удаление элемента из модели
        $scope.delete = function (currentPerson) {
            $http({
                method: "DELETE",
                url: baseUrl + "/" + currentPerson.Id
            }).success(function () {
                var indexForDelete = ArrayHelper.findIndexByKeyValue($scope.persons, "Id", currentPerson.Id);
                $scope.persons.splice(indexForDelete, 1);

                $scope.FullName = "";
                $scope.Login = "";
                $scope.Password = "";
            });
        }

        // сохранение изменений
        $scope.saveEdit = function (currentPerson) {
            if (angular.isDefined(currentPerson.Id)) {
                $scope.update(currentPerson);
            } else {
                $scope.create(currentPerson);
            }
        }

        $scope.update = function (person) {
            var modifiedPerson = angular.copy(person);

            modifiedPerson.FullName = $scope.FullName;
            modifiedPerson.Login = $scope.Login;
            modifiedPerson.Password = $scope.Password;

            if (!$scope.isValid(modifiedPerson))
                return;

            $http({
                url: baseUrl,
                method: "PUT",
                data: modifiedPerson
            }).success(function (modifiedPerson) {
                for (var i = 0; i < $scope.persons.length; i++) {
                    if ($scope.persons[i].Id === modifiedPerson.Id) {
                        $scope.persons[i] = modifiedPerson;
                        break;
                    }
                }
            });
            $scope.refresh();
        }

        //событие выбора пользователя
        $scope.change = function (selected) {
            $scope.currentPerson = selected[0];

            $scope.isValid($scope.currentPerson);

            $scope.FullName = $scope.currentPerson.FullName;
            $scope.Login = $scope.currentPerson.Login;
            $scope.Password = $scope.currentPerson.Password;

            $scope.roles.forEach(function (r) {
                var isPersonHasRole = $scope.currentPerson.Roles.some(function (role, index, array) {
                    return role.Id === r.Id;
                });

                if (isPersonHasRole) {
                    r.isActive = true;
                } else {
                    r.isActive = false;
                }
            });
        }

        $scope.changeRole = function (role) {
            var findedRoleInSelected = ArrayHelper.findIndexByKeyValue($scope.currentPerson.Roles, "Id", role.Id);

            if (role.isActive) {
                if (findedRoleInSelected === null) {
                    $scope.currentPerson.Roles.push(role);
                }
            }

            if (!role.isActive) {
                if (findedRoleInSelected !== null) {
                    $scope.currentPerson.Roles.splice(findedRoleInSelected, 1);
                }
            }
        }

        $scope.isValid = function (newPers) {
            if (!newPers.FullName) {
                $scope.FullNameEmpty = true;
                return false;
            } else
                $scope.FullNameEmpty = false;

            if (!newPers.Login) {
                $scope.LoginEmpty = true;
                return false;
            } else
                $scope.LoginEmpty = false;

            if (!newPers.Password) {
                $scope.PasswordEmpty = true;
                return false;
            }
            else
                $scope.PasswordEmpty = false;

            return true;
        }

        $scope.refresh();
    });