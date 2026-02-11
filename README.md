# \# Otimização de Ray Casting com BVH (Unity)

# 

# Este projeto implementa um sistema de detecção de colisão otimizado para jogos FPS, utilizando \*\*Bounding Volume Hierarchies (BVH)\*\*. 

# O objetivo é demonstrar o ganho de performance ao utilizar volumes delimitadores hierárquicos em comparação com a verificação de força bruta (Mesh Colliders diretos).

# 

# \*\*Disciplina:\*\* Jogos para Consoles  

# \*\*Assunto:\*\* Verificação Suplementar – Ray Casting com BVH  

# \*\*Status:\*\* ✅ Concluído

# 

# ---

# 

# \## Controles do Projeto

# 

# Para realizar os testes de performance e visualização na cena, utilize os seguintes controles:

# 

# | Tecla / Botão | Ação | Descrição Técnica |

# | :--- | :--- | :--- |

# | \*\*Botão Esquerdo\*\* (Mouse) | \*\*Disparo Otimizado (BVH)\*\* | Realiza o Raycast verificando primeiro a Raiz do BVH. Desenha uma linha \*\*VERDE\*\* se acertar. |

# | \*\*Botão Direito\*\* (Mouse) | \*\*Disparo Força Bruta\*\* | Realiza o Raycast testando diretamente todas as malhas (Mesh Colliders). Desenha uma linha \*\*VERMELHA\*\* se acertar. |

# | \*\*Botão do Meio\*\* (Scroll) | \*\*Comparação Instantânea\*\* | Dispara ambos os métodos no mesmo frame e exibe no Console o ganho exato de performance. |

# | \*\*Tecla `T`\*\* | \*\*Teste de Estresse\*\* | Executa um loop de milhares de disparos aleatórios para gerar uma média de tempo (ms) no Console. |

# | \*\*Mouse\*\* | \*\*Mirar\*\* | Mira para selecionar os alvos. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `W`\*\* | \*\*Movimento\*\* | Movimenta a câmera na horizontal. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `A`\*\* | \*\*Movimento\*\* | Movimenta a câmera na horizontal. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `S`\*\* | \*\*Movimento\*\* | Movimenta a câmera na horizontal. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `D`\*\* | \*\*Movimento\*\* | Movimenta a câmera na horizontal. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `Q`\*\* | \*\*Movimento\*\* | Movimenta a câmera na vertical. (Precisa ativar o script PlayerController na Main Camera) |

# | \*\*Tecla `E`\*\* | \*\*Movimento\*\* | Movimenta a câmera na vertical. (Precisa ativar o script PlayerController na Main Camera) |

# ---

# 

# \## Visualização (Debug)

# 

# O projeto utiliza \*\*Gizmos\*\* e linhas de debug para facilitar a compreensão visual da hierarquia:

# 

# \### Na Cena (Gizmos)

# \* 🟩 \*\*Caixa Verde (Grande):\*\* Representa a \*\*Raiz (Root)\*\* do BVH. Se o raio não tocar nesta caixa, o inimigo inteiro é ignorado.

# \* 🟨 \*\*Caixa Amarela (Pequena):\*\* Representa os \*\*Nós Folha (Leaf Nodes)\*\*, ou seja, as partes individuais do corpo (Cabeça, Tronco, Membros).

# 

# \### No Disparo (Ray Lines)

# \* 🟢 \*\*Linha Verde:\*\* Indica um acerto processado pelo algoritmo \*\*Com BVH\*\*.

# \* 🔴 \*\*Linha Vermelha:\*\* Indica um acerto processado pelo algoritmo \*\*Sem BVH\*\*.

# 

# ---

# 

# \## Como Testar a Performance

# 

# 1\.  Dê \*\*Play\*\* na cena `CenaPrincipal`.

# 2\.  Certifique-se de que a janela \*\*"Console"\*\* e \*\*"Game"\*\* estão visíveis.

# 3\.  Ative o botão \*\*"Gizmos"\*\* na janela Game para ver as caixas.

# 4\.  \*\*Teste Individual:\*\* Mire em um inimigo e clique com os botoes do mouse mostrados nos controles. Leia o log no Console.

# 5\.  \*\*Teste Geral:\*\* Pressione a tecla \*\*`T`\*\* e verifique o tempo total em milissegundos no Console.

# 

# ---

# 

# \## Detalhes da Implementação

# 

# \* \*\*Motor:\*\* Unity version 6000.3.3f1 (C#)

# \* \*\*Cenário:\*\* Mínimo de 50 inimigos instanciados.

# \* \*\*Modelos:\*\* 3 variações de Prefabs com >3.000 polígonos cada.

# \* \*\*Colisão:\*\* Verificação final realizada a nível de malha (`MeshCollider`).

# 

# ---

# 

# \*\*Autor:\*\* Joao Vitor Ribeiro

