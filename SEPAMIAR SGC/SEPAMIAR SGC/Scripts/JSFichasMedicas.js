$(document).ready(function () {
    var json = new Object();
    var foodRegimesCount = getRegimesCount();
    var controlsTablesRowsCount = getControlsTableRowsCount();

    function getControlsTableRowsCount() {
        $tableRows = $("#controlsTable tr");

        var rowsCount = $tableRows.length;

        return rowsCount;
    }

    function getRegimesCount() {
        var regimesCount = $("#regimeInitialCount").val();

        if (regimesCount != null) {
            return regimesCount;
        } else {
            return 0;
        }
    }

    $('#create-mh').on("click", function () {
        var spinner = new Spinner().spin()
        $("#save-btn-container").append(spinner.el)

        var form = $('#c-1')
        var form_2 = $('#c-2')
        var form_3 = $('#c-3')
        var form_4 = $('#c-4')

        // C_1

        json.programId = form.find('#programa_id').val();
        json.clientId = form.find('#cliente_id').val();
        json.height = form.find('#estatura').val();
        json.contexture = form.find('#contextura').val();
        json.actualWeight = form.find('#peso_actual').val();
        json.wishedWeight = form.find('#peso_deseado').val();
        json.idealWeight = form.find('#peso_ideal').val();

        // C_2

        var controlsArray = new Array();

        for (var i = 1; i < getControlsTableRowsCount(); i++) {
            var control = new Object();
            var row = "#r-" + (i-1).toString();
            control.date = form_2.find(row + "-fecha").val();
            control.act = form_2.find(row + '-act').val();
            control.mme = form_2.find(row + '-mme').val();
            control.mgc = form_2.find(row + '-mgc').val();
            control.imc = form_2.find(row + '-mc').val();
            control.pgc = form_2.find(row + '-pgc').val();
            control.rcc = form_2.find(row + '-rcc').val();
            control.weight = form_2.find(row + '-peso').val();

            controlsArray.push(control);
        }

        json.controls = controlsArray;

        // C_3

        json.smokeFrecuency = form_3.find('#frec_fumador').val();
        json.drinkFrecuency = form_3.find('#frec_bebedor').val();
        json.cortisone = form_3.find('#cortisona').is(':checked');
        json.anabolics = form_3.find('#esteroidesAnabolicos').is(':checked');
        json.anfetamines = form_3.find('#anfetaminas').is(':checked');
        json.marihuana = form_3.find('#marihuana').is(':checked');
        json.other = form_3.find('#otra').is(':checked');
        json.none = form_3.find('#ninguna').is(':checked');
        json.childQty = form_3.find('#menstruacion_menopausia_cantidad_hijos').val();
        json.birthType = form_3.find('#menstruacion_menopausia_tipo_parto').val();
        json.menopauseAge = form_3.find('#menstruacion_menopausia_edad_menopausia').val();
        json.menopauseActive = form_3.find('#menstruacion_menopausia_activa').is(':checked');
        json.hormoneTreatment = form_3.find('#menstruacion_menopausia_tratamiento_hormonal').val();
        json.bgGastric = form_3.find('#antecedentes_gastricos').is(':checked');
        json.bgDiabetes = form_3.find('#antecedentes_diabetes').is(':checked');
        json.bgHighTriglicerids = form_3.find('#antecedentes_trigliceridos_altos').is(':checked');
        json.bgHipertiroidism = form_3.find('#antecedentes_hipertiroidismo').is(':checked');
        json.bgHipotiroidism = form_3.find('#antecedentes_hipotiroidismo').is(':checked');
        json.bgQuists = form_3.find('#antecedentes_quistes').is(':checked');
        json.bgTumors = form_3.find('#antecedentes_tumores').is(':checked');
        json.bgInsulinResistant = form_3.find('#antecedentes_resistencia_insulina').is(':checked');
        json.bgOtherDyslipidemias = form_3.find('#antecedentes_otras_dislipidemias').is(':checked');
        json.bgProstate = form_3.find('#antecedentes_prostata').is(':checked');
        json.bgRenal = form_3.find('#antecedentes_renales').is(':checked');
        json.bgHepatic = form_3.find('#antecedentes_hepaticos').is(':checked');
        json.bgHighPresure = form_3.find('#antecedentes_presion_alta').is(':checked');
        json.bgLowPresure = form_3.find('#antecedentes_presion_baja').is(':checked');
        json.bgHighCholesterol = form_3.find('#antecedentes_colesterol_alto').is(':checked');
        json.bgAnemia = form_3.find('#antecedentes_anemia').is(':checked');
        json.bgHernia = form_3.find('#antecedentes_hernias').is(':checked');
        json.bgConstipation = form_3.find('#antecedentes_estrenimiento').is(':checked');
        json.bgNotMentioned = form_3.find('#antecedentes_no_mencionada').is(':checked');
        json.traumaArticulationPain = form_3.find('#problemas_traumatologicos_dolores_articulares').is(':checked');
        json.traumaOsteoporosis = form_3.find('#problemas_traumatologicos_osteoporosis').is(':checked');
        json.traumaColumnProblems = form_3.find('#problemas_traumatologicos_problemas_columna').is(':checked');
        json.traumaColumnProblemsText = form_3.find('#problemas_traumatologicos_problemas_columna_texto').val();
        json.traumaOtherInjuries = form_3.find('#problemas_traumatologicos_otras_lesiones').is(':checked');
        json.traumaOtherInjuriesText = form_3.find('#problemas_traumatologicos_otras_lesiones_texto').val();
        json.respiratoryChronicAsthma = form_3.find('#problemas_respiratorios_asma_cronica').is(':checked');
        json.respiratoryChronicCough = form_3.find('#problemas_respiratorios_tos_cronica').is(':checked');
        json.respiratoryContinuosColds = form_3.find('#problemas_respiratorios_resfriados_continuos').is(':checked');
        json.respiratoryBronquitis = form_3.find('#problemas_respiratorios_bronquitis').is(':checked');
        json.respiratoryRinitis = form_3.find('#problemas_respiratorios_rinitis_alergica').is(':checked');
        json.respiratorySinusitis = form_3.find('#problemas_respiratorios_sinusitis').is(':checked');
        json.respiratoryAmigdalitis = form_3.find('#problemas_respiratorios_amigdalitis').is(':checked');
        json.respiratoryOther = form_3.find('#problemas_respiratorios_otros').is(':checked');
        json.gbgObesity = form_3.find('#antecedentas_generales_obesidad').is(':checked');
        json.gbgHipertension = form_3.find('#antecedentas_generales_hipertension').is(':checked');
        json.gbgDiabetes = form_3.find('#antecedentas_generales_diabetes').is(':checked');
        json.gbgCardiopathy = form_3.find('#antecedentas_generales_cardiopatias').is(':checked');
        json.gbgNotMentioned = form_3.find('#antecedentas_generales_no_mencionada').is(':checked');
        json.anamnesisFoodOutOfHome = form_3.find('#anamnesis_nutricional_alimentos_fuera_de_casa').is(':checked');
        json.anamnesisFoodOutOfHomeFrec = form_3.find('#anamnesis_nutricional_frecAlimentosFuera').val();
        json.anamnesisFoodOutOfHomeType = form_3.find('#anamnesis_nutricional_tipo_alimentos_fuera').val();
        json.anamnesisWaterGlassQty = form_3.find('#anamnesis_nutricional_cantidad_vasos_agua_dia').val();
        json.anamnesisPreferredFood = form_3.find('#anamnesis_nutricional_alimentos_preferencia').val();
        json.anamnesisNonPreferredFood = form_3.find('#anamnesis_nutricional_alimentos_no_preferencia').val();
        json.anamnesisUnsafeFood = form_3.find('#anamnesis_nutricional_alimentos_daninos').val();

        // C_4

        var foodRegimesArray = new Array();

        for (var j = 0; j < foodRegimesCount; j++) {
            var foodRegime = new Object();
            var fr = "#fr-" + j.toString();
            foodRegime.foodType = form_4.find(fr + "-Select-type").val();
            foodRegime.hour = form_4.find(fr + '-Time-hour').val();
            foodRegime.detail = form_4.find(fr + '-Detail-detail').val();

            foodRegimesArray.push(foodRegime);
        }

        json.foodRegimes = foodRegimesArray;

        var jsonAsString = JSON.stringify(json);
        console.log(jsonAsString);

        var posting = $.post("/FichasMedicas/Create", { json: jsonAsString });

        posting.done(function (data) {
            window.location.href = "/FichasMedicas/Index";
        })

        posting.fail(function (data) {
            spinner.stop();
            console.log(data)
            alert(data);
        })
    })

    $('#edit-mh').on("click", function () {

        var spinner = new Spinner().spin()
        $("#save-btn-container").append(spinner.el)

        var form = $('#c-1')
        var form_2 = $('#c-2')
        var form_3 = $('#c-3')
        var form_4 = $('#c-4')

        // C_1

        json.fmId = form.find('#id').val()
        json.programId = form.find('#programa_id').val();
        json.clientId = form.find('#cliente_id').val();
        json.height = form.find('#estatura').val();
        json.contexture = form.find('#contextura').val();
        json.actualWeight = form.find('#peso_actual').val();
        json.wishedWeight = form.find('#peso_deseado').val();
        json.idealWeight = form.find('#peso_ideal').val();

        // C_2

        var controlsArray = new Array();

        for (var i = 0; i < controlsTablesRowsCount; i++) {
            var control = new Object();
            var row = "#r-" + i.toString();
            control.date = form_2.find(row + "-fecha").val();
            control.act = form_2.find(row + '-act').val();
            control.mme = form_2.find(row + '-mme').val();
            control.mgc = form_2.find(row + '-mgc').val();
            control.imc = form_2.find(row + '-mc').val();
            control.pgc = form_2.find(row + '-pgc').val();
            control.rcc = form_2.find(row + '-rcc').val();
            control.weight = form_2.find(row + '-peso').val();
            control.id = form_2.find(row + '-id').val();
            control.created_at = form_2.find(row + '-created-at').val();

            controlsArray.push(control);
        }

        json.controls = controlsArray;

        // C_3

        json.smokeFrecuency = form_3.find('#frec_fumador').val();
        json.drinkFrecuency = form_3.find('#frec_bebedor').val();
        json.cortisone = form_3.find('#cortisona').is(':checked');
        json.anabolics = form_3.find('#esteroidesAnabolicos').is(':checked');
        json.anfetamines = form_3.find('#anfetaminas').is(':checked');
        json.marihuana = form_3.find('#marihuana').is(':checked');
        json.other = form_3.find('#otra').is(':checked');
        json.none = form_3.find('#ninguna').is(':checked');
        json.childQty = form_3.find('#menstruacion_menopausia_cantidad_hijos').val();
        json.birthType = form_3.find('#menstruacion_menopausia_tipo_parto').val();
        json.menopauseAge = form_3.find('#menstruacion_menopausia_edad_menopausia').val();
        json.menopauseActive = form_3.find('#menstruacion_menopausia_activa').is(':checked');
        json.hormoneTreatment = form_3.find('#menstruacion_menopausia_tratamiento_hormonal').val();
        json.bgGastric = form_3.find('#antecedentes_gastricos').is(':checked');
        json.bgDiabetes = form_3.find('#antecedentes_diabetes').is(':checked');
        json.bgHighTriglicerids = form_3.find('#antecedentes_trigliceridos_altos').is(':checked');
        json.bgHipertiroidism = form_3.find('#antecedentes_hipertiroidismo').is(':checked');
        json.bgHipotiroidism = form_3.find('#antecedentes_hipotiroidismo').is(':checked');
        json.bgQuists = form_3.find('#antecedentes_quistes').is(':checked');
        json.bgTumors = form_3.find('#antecedentes_tumores').is(':checked');
        json.bgInsulinResistant = form_3.find('#antecedentes_resistencia_insulina').is(':checked');
        json.bgOtherDyslipidemias = form_3.find('#antecedentes_otras_dislipidemias').is(':checked');
        json.bgProstate = form_3.find('#antecedentes_prostata').is(':checked');
        json.bgRenal = form_3.find('#antecedentes_renales').is(':checked');
        json.bgHepatic = form_3.find('#antecedentes_hepaticos').is(':checked');
        json.bgHighPresure = form_3.find('#antecedentes_presion_alta').is(':checked');
        json.bgLowPresure = form_3.find('#antecedentes_presion_baja').is(':checked');
        json.bgHighCholesterol = form_3.find('#antecedentes_colesterol_alto').is(':checked');
        json.bgAnemia = form_3.find('#antecedentes_anemia').is(':checked');
        json.bgHernia = form_3.find('#antecedentes_hernias').is(':checked');
        json.bgConstipation = form_3.find('#antecedentes_estrenimiento').is(':checked');
        json.bgNotMentioned = form_3.find('#antecedentes_no_mencionada').is(':checked');
        json.traumaArticulationPain = form_3.find('#problemas_traumatologicos_dolores_articulares').is(':checked');
        json.traumaOsteoporosis = form_3.find('#problemas_traumatologicos_osteoporosis').is(':checked');
        json.traumaColumnProblems = form_3.find('#problemas_traumatologicos_problemas_columna').is(':checked');
        json.traumaColumnProblemsText = form_3.find('#problemas_traumatologicos_problemas_columna_texto').val();
        json.traumaOtherInjuries = form_3.find('#problemas_traumatologicos_otras_lesiones').is(':checked');
        json.traumaOtherInjuriesText = form_3.find('#problemas_traumatologicos_otras_lesiones_texto').val();
        json.respiratoryChronicAsthma = form_3.find('#problemas_respiratorios_asma_cronica').is(':checked');
        json.respiratoryChronicCough = form_3.find('#problemas_respiratorios_tos_cronica').is(':checked');
        json.respiratoryContinuosColds = form_3.find('#problemas_respiratorios_resfriados_continuos').is(':checked');
        json.respiratoryBronquitis = form_3.find('#problemas_respiratorios_bronquitis').is(':checked');
        json.respiratoryRinitis = form_3.find('#problemas_respiratorios_rinitis_alergica').is(':checked');
        json.respiratorySinusitis = form_3.find('#problemas_respiratorios_sinusitis').is(':checked');
        json.respiratoryAmigdalitis = form_3.find('#problemas_respiratorios_amigdalitis').is(':checked');
        json.respiratoryOther = form_3.find('#problemas_respiratorios_otros').is(':checked');
        json.gbgObesity = form_3.find('#antecedentas_generales_obesidad').is(':checked');
        json.gbgHipertension = form_3.find('#antecedentas_generales_hipertension').is(':checked');
        json.gbgDiabetes = form_3.find('#antecedentas_generales_diabetes').is(':checked');
        json.gbgCardiopathy = form_3.find('#antecedentas_generales_cardiopatias').is(':checked');
        json.gbgNotMentioned = form_3.find('#antecedentas_generales_no_mencionada').is(':checked');
        json.anamnesisFoodOutOfHome = form_3.find('#anamnesis_nutricional_alimentos_fuera_de_casa').is(':checked');
        json.anamnesisFoodOutOfHomeFrec = form_3.find('#anamnesis_nutricional_frecAlimentosFuera').val();
        json.anamnesisFoodOutOfHomeType = form_3.find('#anamnesis_nutricional_tipo_alimentos_fuera').val();
        json.anamnesisWaterGlassQty = form_3.find('#anamnesis_nutricional_cantidad_vasos_agua_dia').val();
        json.anamnesisPreferredFood = form_3.find('#anamnesis_nutricional_alimentos_preferencia').val();
        json.anamnesisNonPreferredFood = form_3.find('#anamnesis_nutricional_alimentos_no_preferencia').val();
        json.anamnesisUnsafeFood = form_3.find('#anamnesis_nutricional_alimentos_daninos').val();

        // C_4

        var foodRegimesArray = new Array();

        for (var j = 0; j < foodRegimesCount; j++) {
            var foodRegime = new Object();
            var fr = "#fr-" + j.toString();
            foodRegime.foodType = form_4.find(fr + "-Select-type").val();
            foodRegime.hour = form_4.find(fr + '-Time-hour').val();
            foodRegime.detail = form_4.find(fr + '-Detail-detail').val();
            foodRegime.id = form_4.find(fr + '-Id-id').val();

            foodRegimesArray.push(foodRegime);
        }

        json.foodRegimes = foodRegimesArray;

        var jsonAsString = JSON.stringify(json);
        console.log(jsonAsString);

        var posting = $.post("/FichasMedicas/Edit", { json: jsonAsString });

        posting.done(function (data) {
            window.location.href = "/FichasMedicas/Index";
        })

        posting.fail(function (data) {
            spinner.stop();
            console.log(data)
            alert(data);
        })
    })

    $('#addFoodRegime').on("click", function () {
        var frModalId = "fr-" + foodRegimesCount.toString();
        var frModalIdSelectDiv = frModalId + "-Select"
        var frModalIdTimeDiv = frModalId + "-Time"
        var frModalIdDetailDiv = frModalId + "-Detail"
        var frModalIdIdDiv = frModalId + "-Id"

        $('#regimenComida').append("<div id='" + frModalId + "' class='model food-regime new'></div>");
        $('#' + frModalId).append("<div id='" + frModalIdSelectDiv + "' class='mar-bot-10'></div>");
        $('#' + frModalIdSelectDiv).append("<select id='" + frModalIdSelectDiv + "-type' class='form-control search mar-0' name='tipo_comida'><option selected='selected' value='0'>Desayuno</option><option value='1'>MediaMañana</option><option value='2'>Almuerzo</option><option value='3'>MediaTarde</option><option value='4'>Cena</option></select>");

        $('#' + frModalId).append("<div id='" + frModalIdTimeDiv + "' class='mar-bot-10'></div>");
        $('#' + frModalIdTimeDiv).append("<input id='" + frModalIdTimeDiv + "-hour' type='text' name='hora' value='' class='form-control search mar-0 timepicker' />");
        $('#' + frModalIdTimeDiv).find('.timepicker').on("click", function () {
            $('.timepicker').timepicker({
                timeFormat: 'hh:mm',
                interval: 15,
                minTime: '00:00',
                maxTime: '10:00pm',
                defaultTime: '8',
                startTime: '00:00',
                dynamic: false,
                dropdown: true,
                scrollbar: true
            });
        })
        $('#' + frModalId).append("<div id='" + frModalIdDetailDiv + "' class='mar-bot-10'></div>");
        $('#' + frModalIdDetailDiv).append("<textarea class='form-control search mar-0' cols='20' id='" + frModalIdDetailDiv + "-detail' name='detalles' rows='2'></textarea>");
        $('#' + frModalId).append("<div id='" + frModalIdIdDiv + "' class='mar-bot-10'></div>");
        $('#' + frModalIdIdDiv).append("<textarea class='form-control search mar-0' cols='20' id='" + frModalIdIdDiv + "-id' name='id' rows='2' style='display: none;'></textarea>");


        foodRegimesCount++;
    });

    $('#addControlsRow').on("click", function () {
        $table = $("#controlsTable").find('tbody')
        var rowCount = controlsTablesRowsCount - 1
        $tr = $("<tr id='r-" + rowCount.toString() + "' class='new'></tr>");

        $tr.append("<td><input id='r-" + rowCount.toString() + "-fecha' type='text' name='Fecha' value='' class='form-control search mar-0 datepicker' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-act' type='text' name='ACT' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-mme' type='text' name='MME' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-mgc' type='text' name='MGC' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-mc' type='text' name='MC' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-pgc' type='text' name='PGC' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-rcc' type='text' name='RCC' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td><input id='r-" + rowCount.toString() + "-peso' type='text' name='Peso' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td style='display: none;'><input id='r-" + rowCount.toString() + "-id' type='text' name='id' value='' class='form-control search mar-0' /></td>");
        $tr.append("<td style='display: none;'><input id='r-" + rowCount.toString() + "-created-at' type='text' name='created_at' value='' class='form-control search mar-0' /></td>");


        $tr.find('.datepicker').on("click", function () {
            $('.datepicker').datepicker();
        })

        $table.append($tr);
        controlsTablesRowsCount++;
    });

    $('.timepicker').timepicker({
        timeFormat: 'hh:mm',
        interval: 15,
        minTime: '00:00',
        maxTime: '10:00pm',
        defaultTime: '8',
        startTime: '00:00',
        dynamic: false,
        dropdown: true,
        scrollbar: true
    });
});