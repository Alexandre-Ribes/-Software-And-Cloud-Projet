Maintient la position du serpent

Pour le maintien de la position du serpent, on va récupérer sa position actuelle et sa position précédente : imaginons que nous avons au départ la tête du serpent en [7,5] et ensuite il se déplace en [7,4] car on a reçu la direction “up” sauf qu’ensuite on reçoit la direction “none” cela veut dire qu’il n’y a eu aucune direction de donné dans ce cas la, vu qu’il s’est déplacé vers le haut avant alors il se déplacera automatiquement vers le haut

Met à jour la position après chaque déplacement

Le mouvement va récupérer la direction envoyée par l’input ensuite une fonction prenant en compte le paramètre envoyé par l’input va être mise en place dans cette fonction 
si la direction est “up” alors on fera -1 a la var y
si la direction est “down” alors on fera +1 a la var y
si la direction est “right” alors on fera +1 a la var x
si la direction est “left” on fera -1 a la var x
Ensuite on va ajouter la nouvelle valeur x et y a la liste et supprimer la première valeur x et y présente dans la liste.

Publie un événement Snake.moved

Enfin on va publier un événement Snake.moved qui sera récupéré par le service display et le service collision
service collision : le service collision a besoin de savoir si la nouvelle position rentre en collision avec les limites de la grille donc l’évent sera récupéré par cette classe
service display : le service display et celui qui récupère toute les données des différents microservices il va donc récupérer les données transmises par l’événement du service Snake State


