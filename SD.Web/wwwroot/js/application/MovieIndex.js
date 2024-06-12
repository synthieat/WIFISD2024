$(document).ready(function ()
{
    const MOVIETABLE = 'MovieTable';
    const MOVIEDETAILSMODAL = 'MovieDetailsModal';

    //var movieTable = document.querySelector('#MovieTable');

    var $movieTable = $('#' + MOVIETABLE);

    // Suche nach Hyperlinks mit class fa-remove
    
    var $deleteHyperLinks = $movieTable.find('a.fa-remove');

    // Suche nach data-name Attribute mit Wert Delete
    $deleteHyperLinks = $movieTable.find('a[data-name=Delete]');

    $deleteHyperLinks.on('click', function () {
        var rowsToShow = [2, 0, 1, 5];

        ShowDetailsModal(MOVIETABLE, MOVIEDETAILSMODAL, this, true, rowsToShow );
        return false;
    })    

    var $detailsHyperLinks = $movieTable.find('a[data-name=Details]');
    $detailsHyperLinks.on('click', function () {
        var rowsToShow = [0, 1, 2, 3, 4, 5];
        ShowDetailsModal(MOVIETABLE, MOVIEDETAILSMODAL, this, false, rowsToShow);
        return false;
    })

    var $editHyperLinks = $movieTable.find('a[data-name=Edit]');
    $editHyperLinks.on('click', function () {

        /* Wert aus data-Id in modal id schreiben */
        var idValue = $(this).attr('data-id');


        var $EditMoviePartialView = $("#EditMoviePartialView");
        $EditMoviePartialView.empty();
        var url = "/Movies/Edit/" + idValue;

        /* Version with JQuery 
        $.get(url, function (data) {
            $EditMoviePartialView.html(data);
            $.validator.unobtrusive.parse($EditMoviePartialView);
            ShowMovieEditModal();
        });
        */

        /* Version with Javascript fetch API */
        fetch(url)
            .then(response => { return response.text(); })
            .then(data => $EditMoviePartialView.html(data)) /* Response von Partial View in Container Div einfügen */
            .then(x => $.validator.unobtrusive.parse($EditMoviePartialView)) /* JQuery Validator initialisieren */
            .then(x => ShowMovieEditModal()); /* Modalen Dialog öffnen */
      
        return false;
    })

}) 

async function loadMovie(id) {
    var url = "/Movies/Edit/" + id;
    
    alert(response);    
}

function ShowMovieEditModal () {
    var options =
    {
        "backdrop": "static",
        "keyboard": true
    }

    /* Modalen Dialog initialisieren und aufrufen */
    var movieEditModal = document.getElementById('MovieEditModal');

    var modal = new bootstrap.Modal(movieEditModal, options);
    modal.show();

    return false
}


document.addEventListener("DOMContentLoaded", function ()
{
   // alert("JS method");

})