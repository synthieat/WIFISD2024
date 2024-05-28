$(document).ready(function ()
{
    var $movieTable = $('#MovieTable');

    // Suche nach Hyperlinks mit class fa-remove
    var $deleteHyperLinks = $movieTable.find('a.fa-remove');

    // Suche nach data-name Attribute mit Wert Delete
    $deleteHyperLinks = $movieTable.find('a[data-name=Delete]');

    $deleteHyperLinks.on('click', function () {
        var rowsToShow = [2, 0, 1];

        ShowDetailsModal("MovieTable", "MovieDetailsModal", this, true, rowsToShow );
        return false;
    })    

}) 


document.addEventListener("DOMContentLoaded", function ()
{
   // alert("JS method");

})