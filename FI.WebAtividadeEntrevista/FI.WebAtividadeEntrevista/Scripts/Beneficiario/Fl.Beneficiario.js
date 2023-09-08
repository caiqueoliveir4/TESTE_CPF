let gridBeneficiario = new GridBeneficiario()

function GridBeneficiario() {

    $(document).ready(function () {
        $("#btnBeneficiarios").click(function () {
            $("#modalBeneficiario").modal("show");
        });

        $('#formBeneficiario').submit(function (e) {
            e.preventDefault();

            data =
            {
                "Nome": $("#formBeneficiario input[name='NomeBeneficiario']").val().trim(),
                "Cpf": $("#formBeneficiario input[name='CpfBeneficiario']").val().trim(),
                "Id": $("#formBeneficiario input[name='IdBenefeciario']").val().trim()
            }

            if ($('#formBeneficiario button[type="submit"]').data('action') == 'incluir') {
                AdicionarLinha(data)
            } else {
                var linha = $('#formBeneficiario button[type="submit"]').data('linha')
                AtualizarLinha(data, linha)
                MudaBotoes('incluir')
            }

            $('#formBeneficiario button[type="reset"]').click()

            return false;
        })
    })

    function MudaBotoes(action) {
        if (action == 'incluir') {
            $('#formBeneficiario button[type="submit"]').html('Incluir')
            $('#formBeneficiario button[type="submit"]').data("action", 'incluir')
        } else {
            $('#formBeneficiario button[type="submit"]').html('Alterar')
            $('#formBeneficiario button[type="submit"]').data("action", 'alterar')
        }
    }
    function AdicionarLinha(beneficiario) {
        var el = $(`<tr class="beneficiario-item">
                    <td>${beneficiario.Cpf}</td>
                    <td>${beneficiario.Nome}</td>
                    <td>
                        <button type='button' class="btn btn-sm btn-primary editarBeneficiario">Editar</button>
                        <button type='button' class="btn btn-sm btn-primary excluirBeneficiario">Excluir</button>
                    </td>
                </tr>`);


        $(el).data('beneficiario', beneficiario);
        $(el).find('.editarBeneficiario').click(() => {
            const beneficiario = $(el).data('beneficiario')
            $("#formBeneficiario input[name='NomeBeneficiario']").val(beneficiario.Nome)
            $("#formBeneficiario input[name='CpfBeneficiario']").val(beneficiario.Cpf)
            $("#formBeneficiario input[name='IdBenefeciario']").val(beneficiario.Id)
            $('#formBeneficiario button[type="submit"]').data('linha', $(el).data('linha'))
            MudaBotoes('alterar')
        });
        $(el).find('.excluirBeneficiario').click(() => {
            $(el).remove()
            OrdernarGrid()
        });

        $('#beneficiariosGrid tbody').append(el);
        OrdernarGrid();
    }

    function AtualizarLinha(beneficiario, linha) {
        var el = $(".beneficiario-item")[linha]
        var tds = $(el).find('td')
        $(tds[0]).html(beneficiario.Cpf)
        $(tds[1]).html(beneficiario.Nome)
        $(el).data('beneficiario', beneficiario)
        OrdernarGrid()
    }

    function OrdernarGrid() {
        var elementos = $(".beneficiario-item");

        elementos.each((_, el) => $(el).detach())
        elementos = elementos.sort((a, b) => $(a).data('beneficiario').Nome.localeCompare($(b).data('beneficiario').Nome))
        elementos.each((linha, el) => {
            $(el).data("linha", linha)
            $(el).appendTo("#beneficiariosGrid tbody")
        })
    }

    function ObterBeneficiarios() {
        var elementos = $(".beneficiario-item");
        var beneficiarios = []
        elementos.each((_, el) => beneficiarios.push($(el).data('beneficiario')))
        return beneficiarios
    }

    function LimpaTabela() {
        $("#beneficiariosGrid tbody").html('')
    }

    function InicialiazaTabela(beneficiarios) {
        LimpaTabela()
        beneficiarios.forEach(beneficiario => AdicionarLinha(beneficiario))
    }

    this.incializaTabela = InicialiazaTabela
    this.obterBeneficiarios = ObterBeneficiarios
}


