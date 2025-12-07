document.addEventListener('click', function(e){
    if(e.target.matches('form button[type="submit"]') && e.target.closest('form')?.getAttribute('asp-action') === 'Import'){
        if(!confirm('Importar este filme para o catálogo local?')) {
            e.preventDefault();
        }
    }
});
