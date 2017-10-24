$(document).ready(function () {
    var dietOptions = new Array();

    var createdOptions = GetOptionsInitialCount();
    var dietsDiv = $("#diets");

    function GetOptionsInitialCount() {
        var count = $("#OptionsInitialCount").val();

        if (count != null) {
            return count;
        } else {
            return 0;
        }
    }

    $('#addDietOption').on("click", function () {
        var newDietOptionLabelRow = "<div class='row mar-bot-10'><label class='diet-title-sub'>Opción " + (createdOptions + 1).toString() + "</label></div>";
        dietsDiv.append(newDietOptionLabelRow);
        var dietDivRowId = "opt-" + createdOptions.toString();
        var newDietDivRow = "<div id='" + dietDivRowId + "' class='row diet-row mar-bot-15'></div>";
        dietsDiv.append(newDietDivRow);

        var dietOptionRow = $("#" + dietDivRowId)

        for (var i = 0; i < 7; i++) {
            var optColId = dietDivRowId + "-col-" + i.toString();
            if (i == 0) {
                var newDietColumn = "<div id='" + optColId + "' class='col-2'></div>";
                dietOptionRow.append(newDietColumn);

                $("#create-diets-btn").attr('disabled', false);
            } else {
                var newDietColumn = "<div id='" + optColId + "' class='col-2 mar-lef-10'></div>";
                dietOptionRow.append(newDietColumn);
            }

            var currentColumn = $("#" + optColId);

            switch (i) {
                case 0:
                    currentColumn.append("<label class='diet-title'>Lun</label>");
                    break;
                case 1:
                    currentColumn.append("<label class='diet-title'>Mar</label>");
                    break;
                case 2:
                    currentColumn.append("<label class='diet-title'>Mie</label>");
                    break;
                case 3:
                    currentColumn.append("<label class='diet-title'>Jue</label>");
                    break;
                case 4:
                    currentColumn.append("<label class='diet-title'>Vie</label>");
                    break;
                case 5:
                    currentColumn.append("<label class='diet-title'>Sab</label>");
                    break;
                case 6:
                    currentColumn.append("<label class='diet-title'>Dom</label>");
                    break;
                default:
                    break;
            }

            for (var j = 0; j < 5; j++) {
                var currentBoxId = optColId + "-row-" + j.toString();
                switch (j) {
                    case 0:
                        currentColumn.append("<div class='model daily-diet'><label>Desayuno</label><textarea id='" + currentBoxId + "'></textarea></div>");
                        currentColumn.append("<input type='hidden' id='"+ currentBoxId + "-id' value='0'/>")
                        break;
                    case 1:
                        currentColumn.append("<div class='model daily-diet'><label>Media Mañana</label><textarea id='" + currentBoxId + "'></textarea></div>");
                        currentColumn.append("<input type='hidden' id='" + currentBoxId + "-id' value='0'/>")
                        break;
                    case 2:
                        currentColumn.append("<div class='model daily-diet'><label>Almuerzo</label><textarea id='" + currentBoxId + "'></textarea></div>");
                        currentColumn.append("<input type='hidden' id='" + currentBoxId + "-id' value='0'/>")
                        break;
                    case 3:
                        currentColumn.append("<div class='model daily-diet'><label>Media Tarde</label><textarea id='" + currentBoxId + "'></textarea></div>");
                        currentColumn.append("<input type='hidden' id='" + currentBoxId + "-id' value='0'/>")
                        break;
                    case 4:
                        currentColumn.append("<div class='model daily-diet'><label>Cena</label><textarea id='" + currentBoxId + "'></textarea></div>");
                        currentColumn.append("<input type='hidden' id='" + currentBoxId + "-id' value='0'/>")
                        break;
                    default:
                        break;
                }
            }
        }

        createdOptions++;
    });

    // "opt-" + createdOptions.toString() + "-col-" + i.toString() + "-row-" + j.toString();

    $("#create-diets-btn").on("click", function () {
        var spinner = new Spinner().spin()
        $("#create-diets-btn").append(spinner.el)

        for (var opt = 0; opt < createdOptions; opt++) {
            for (var col = 0; col < 7; col++) {
                for (var row = 0; row < 5; row++) {
                    var currentBox = "opt-" + opt.toString() + "-col-" + col.toString() + "-row-" + row.toString();
                    var foodBox = new Object();

                    var boxValue = $("#" + currentBox).val();

                    if (boxValue != "") {
                        foodBox.option = opt + 1;
                        foodBox.foodType = row;
                        foodBox.detail = boxValue;
                        foodBox.day = col;

                        dietOptions.push(foodBox);
                    }
                }
            }
        }

        var newDietOptions = new Object();
        newDietOptions.clientId = $("#cId").val();
        newDietOptions.programID = $("#pId").val();
        dietOptions.push(newDietOptions);

        seen = [];

        var jsonAsString = JSON.stringify(dietOptions, function (key, val) {
            if (val != null && typeof val == "object") {
                if (seen.indexOf(val) >= 0) {
                    return;
                }
                seen.push(val);
            }
            return val;
        });

        var posting = $.post("/ClientesDietas/Create", { json: jsonAsString })

        posting.done(function (data) {
            alert("La dieta ha sido asignada, se le redigirá al índice de dietas para el cliente.");
            window.location.href = "/ClientesDietas/GetDietsForClient?value=" + newDietOptions.clientId;
        });

        posting.fail(function (data) {
            spinner.stop();
            console.log(data)
            alert(data);
            dietOptions = new Array();
        });
    });

    $("#edit-diets-btn").on("click", function () {
        var spinner = new Spinner().spin()
        $("#edit-diets-btn").append(spinner.el)

        for (var opt = 0; opt < createdOptions; opt++) {
            for (var col = 0; col < 7; col++) {
                for (var row = 0; row < 5; row++) {
                    var currentBox = "opt-" + opt.toString() + "-col-" + col.toString() + "-row-" + row.toString();
                    var foodBox = new Object();

                    var boxValue = $("#" + currentBox).val();

                    if (boxValue != "") {
                        foodBox.option = opt + 1;
                        foodBox.foodType = row;
                        foodBox.detail = boxValue;
                        foodBox.day = col;
                        var currentBoxId = $("#" + currentBox + "-id");
                        foodBox.id = currentBoxId.val();

                        dietOptions.push(foodBox);
                    }
                }
            }
        }

        var newDietOptions = new Object();
        newDietOptions.clientId = $("#cId").val();
        newDietOptions.programID = $("#programa_clientes_id").val();
        newDietOptions.dietID = $("#id").val();
        dietOptions.push(newDietOptions);

        seen = [];

        var jsonAsString = JSON.stringify(dietOptions, function (key, val) {
            if (val != null && typeof val == "object") {
                if (seen.indexOf(val) >= 0) {
                    return;
                }
                seen.push(val);
            }
            return val;
        });

        var posting = $.post("/ClientesDietas/Edit", { json: jsonAsString })

        posting.done(function (data) {
            alert("La dieta ha sido editada, se le redigirá al índice de dietas para el cliente.");
            window.location.href = "/ClientesDietas/Index?clientCode=" + newDietOptions.clientId;
        });

        posting.fail(function (data) {
            spinner.stop();
            console.log(data)
            alert("No fue posible guardar los cambios efectuados, actualice la página, intente nuevamente o comuníquese con el área de soporte.");
            dietOptions = new Array();
        });
    });
});