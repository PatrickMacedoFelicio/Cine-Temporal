$(document).ready(function () {

    // Abrir modal de detalhes
    $(document).on("click", ".js-open-details", function () {
        const id = $(this).data("id");

        $.ajax({
            url: "/Movies/Details/" + id,
            type: "GET",
            headers: { "X-Requested-With": "XMLHttpRequest" },
            success: function (html) {
                $("#modal-container").html(html);
                $("#modalMovieDetails").modal("show");
            },
            error: function () {
                alert("Erro ao carregar detalhes.");
            }
        });
    });

    // Abrir modal de exclusão
    $(document).on("click", ".js-open-delete", function () {
        const id = $(this).data("id");

        $.ajax({
            url: "/Movies/Delete/" + id,
            type: "GET",
            headers: { "X-Requested-With": "XMLHttpRequest" },
            success: function (html) {
                $("#modal-container").html(html);
                $("#modalDeleteFilme").modal("show");
            },
            error: function () {
                alert("Erro ao carregar confirmação de exclusão.");
            }
        });
    });

});
