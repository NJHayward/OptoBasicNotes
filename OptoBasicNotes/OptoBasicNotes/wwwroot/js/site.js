$(document).ready(function () {
    loadPartial();

    $('#btnShowCreateNote').click(function () {
        hideSuccessCreated();

        var createFormElm = $('#btnCreateNoteForm');
        if (createFormElm.hasClass("collapse")) {
            createFormElm.removeClass("collapse");
            $('#btnShowCreateNote').text("Hide");
        }
        else {
            hideCreateForm();
        }
    });

    $('.catCheck').change(function () {
        removeCategoryError();
    });

    $('#btnCreateNote').click(function () {
        var finishedLooking = false;
        var somethingSelected = false;
        var i = 0;
        do {
            var elm = $("#category-" + i);
            if (elm.length == 0) {
                finishedLooking = true;
            }
            else {
                if (elm.is(":checked")) {
                    somethingSelected = true;
                }
            }

            i++;
        }
        while (!finishedLooking && !somethingSelected)

        if (!somethingSelected) {
            showCategoryError();
        }

        if ($("#createNoteForm").valid() && somethingSelected) {
            var data = $('#createNoteForm').serialize();

            $.ajax({
                type: "post",
                url: "/Home/CreateNote",
                data: data,
                datatype: "json",
                cache: false,
                success: function (data) {
                    if (data) {
                        loadPartial();
                        hideCreateForm();
                        showSuccessCreated();
                    }
                    else {
                        showError();
                    }
                },
                error: function (xhr) {
                    showError();
                }
            });
        }
    });

    function loadPartial() {
        $.ajax({
            url: '/Home/GetNotesPartial',
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            $('#divNotesPartial').html(result);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showError();
        });
    }

    function showError() {
        $('#divErrorPanel').css("display", "block");
    }

    function showCategoryError() {
        $('#divCategoryErrorPanel').css("display", "inline-block");
    }

    function removeCategoryError() {
        $('#divCategoryErrorPanel').css("display", "none");
    }

    function showSuccessCreated() {
        $('#divSuccessCreated').css("display", "block");
    }

    function hideSuccessCreated() {
        $('#divSuccessCreated').css("display", "none");
    }

    function hideCreateForm(createFormElm) {
        $('#btnCreateNoteForm').addClass("collapse");
        $('#btnShowCreateNote').text("Create Note");
    }
});