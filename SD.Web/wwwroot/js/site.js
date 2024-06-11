// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function ShowDetailsModal(tableName, modalName, ctl, showDelete, rowsToShow)
{   
    const DETAILSMODALTITLE = 'DetailsModalTitle';
    const DELETEBUTTON = 'DeleteButton';

    var $row = $(ctl).parent().parent();
    var $columns = $row.find('td');


    var $table = $('#' + tableName);
    var $thRows = $table.find('th');

    var $modal = $('#' + modalName);
    var $modalBody = $modal.find('#DetailsModalBody');

    /* Bestehende Childs löschen */
    $modalBody.empty();

    for (var i = 0; i < rowsToShow.length; i++)
    {
        $modalBody.append($('<dt class="col-md-3">').text($thRows[rowsToShow[i]].innerText));
        $modalBody.append($('<dd class="col-md-9">').text($columns[rowsToShow[i]].innerText));
    }

    
    if (showDelete) {
        var $id = $modal.find('#DetailsModalId');
        /* Wert aus data-Id in modal id schreiben */
        var idValue = $(ctl).attr('data-id');
        $id.val(idValue);

        /* Title dynamisch anpassen */
        var titleText = $('#DeleteTitle').text();
        $('#' + DETAILSMODALTITLE).text(titleText);

        /* Delete Button einblenden mit class="visible" */
        $('#' + DELETEBUTTON).attr("class", "visible btn btn-secondary");
    } else
    {
        $('#' + DETAILSMODALTITLE).text($('#DetailsTitle').text());
        $('#' + DELETEBUTTON).attr("class", "invisible");
    }


    /* Modalen Dialog initialisieren und aufrufen */
    var options =
    {
        "backdrop": "static",
        "keyboard": true
    }
    var modal = new bootstrap.Modal(document.getElementById(modalName), options);
    modal.show();

}
















//document.addEventListener("DOMContentLoaded", function () {
//    var movieTable = document.getElementById('movieTable');
//    var deleteAnchors1;
//    if (movieTable) {
//        deleteAnchors1 = movieTable.querySelectorAll('a.fa-remove');
//    }

//    deleteAnchors1.forEach(function(e){
//        e.addEventListener("click", function () {
//            alert("hallo world");
//            event.preventDefault();

//        });        
//    })  
//});
