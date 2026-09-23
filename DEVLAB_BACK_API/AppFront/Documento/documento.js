const obterBanco = () => JSON.parse(localStorage.getItem('docsSenai')) || [];
const salvarBanco = (dados) => localStorage.setItem('docsSenai', JSON.stringify(dados));

function salvarDocumento() {
    const cod = document.getElementById('codigoCliente').value;
    const file = document.getElementById('arquivo').files[0];
    const idx = document.getElementById('indexEdicao').value;
    const lista = obterBanco();

    if (!cod) return alert('Insira o código.');

    const lerESalvar = (arqBase64) => {
        const doc = { 
            codigo: cod, 
            nomeArquivo: arqBase64?.nome || lista[idx]?.nomeArquivo, 
            conteudoArquivo: arqBase64?.conteudo || lista[idx]?.conteudoArquivo 
        };
        if (!doc.nomeArquivo) return alert('Selecione um arquivo.');

        idx !== "" ? lista[idx] = doc : lista.push(doc);
        salvarBanco(lista);
        resetarFormulario();
        exibirDados();
    };

    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => lerESalvar({ nome: file.name, conteudo: e.target.result });
        reader.readAsDataURL(file);
    } else {
        lerESalvar();
    }
}

function exibirDados() {
    const lista = obterBanco();
    const res = document.getElementById('result');
    if (!lista.length) return res.innerHTML = '<p>Nenhum documento.</p>';

    res.innerHTML = `<table border="1" style="width:100%; border-collapse:collapse; margin-top:20px;">
        ${lista.map((d, i) => `<tr>
            <td>${d.codigo}</td>
            <td><a href="${d.conteudoArquivo}" download="${d.nomeArquivo}">${d.nomeArquivo}</a></td>
            <td>
                <button onclick="prepararEdicao(${i})">Editar</button>
                <button onclick="deletarDocumento(${i})" style="background:#ff4d4d; color:white; border:none; padding:3px 8px;">Excluir</button>
            </td>
        </tr>`).join('')}
    </table>`;
}


function prepararEdicao(i) {
    const doc = obterBanco()[i];
    document.getElementById('codigoCliente').value = doc.codigo;
    document.getElementById('indexEdicao').value = i;
    document.getElementById('btnEnviar').innerText = 'Atualizar';
}

function deletarDocumento(i) {
    if (!confirm('Excluir?')) return;
    const lista = obterBanco();
    lista.splice(i, 1);
    salvarBanco(lista);
    exibirDados();
    resetarFormulario();
}

function resetarFormulario() {
    document.getElementById('codigoCliente').value = '';
    document.getElementById('arquivo').value = '';
    document.getElementById('indexEdicao').value = '';
    document.getElementById('btnEnviar').innerText = 'Enviar';
}
