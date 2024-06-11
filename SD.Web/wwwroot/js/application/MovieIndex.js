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
    //$editHyperLinks.on('click', function () {

    //    /* Wert aus data-Id in modal id schreiben */
    //    var idValue = $(this).attr('data-id');


    //    var $EditMoviePartialView = $("#EditMoviePartialView");

    //    //$EditMoviePartialView.load("/Movies/Edit/" + idValue);  

    //    /* Modalen Dialog initialisieren und aufrufen */
    //    var options =
    //    {
    //        "backdrop": "static",
    //        "keyboard": true
    //    }
        
        
    //    var modal = new bootstrap.Modal(document.getElementById('MovieEditModal'), options);
    //    modal.show();

    //    return false;
    //})

}) 


document.addEventListener("DOMContentLoaded", function ()
{
   // alert("JS method");

})