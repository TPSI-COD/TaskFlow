let tarefaId = null;

const tarefas = document.querySelectorAll(".tarefa-card");

tarefas.forEach(tarefa => {

    tarefa.addEventListener("dragstart", () => {

        tarefaId = tarefa.dataset.id;

        console.log("Arrastando tarefa:", tarefaId);

    });

    const colunas = document.querySelectorAll(".coluna-kanban");

colunas.forEach(coluna => {

    coluna.addEventListener("dragover", (e) => {

        e.preventDefault();

    });

    coluna.addEventListener("drop", async () => {

    const novoStatus = coluna.dataset.status;

    try {

        const resposta = await fetch(
            `/api/tarefas/${tarefaId}/status`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(novoStatus)
            }
        );

        if (resposta.ok) {

            console.log("Status atualizado!");

            location.reload();

        }
        else {

            console.error(
                "Erro:",
                resposta.status
            );

        }

    }
    catch (erro) {

        console.error(
            "Erro ao chamar API:",
            erro
        );

    }

});

    

    });

});

