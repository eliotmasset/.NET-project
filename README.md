# Projet de bataille navale en .NET

## Participants
 - Masset Eliot
 - Lasserre Julie

## Lancement du projet :

```
cd BattelShip.API/
dotnet ef migration update
dotnet watch
```

```
cd BattelShip.App/
dotnet watch
```

## Liste des fonctionnalités :

 - Jouer contre une IA
 - IA intelligente
 - Personnaliser la partie et déplacer les bateaux
 - Rotations des bateaux
 - Affichage des navires touchés
 - Affichage des navires coulés
 - Affichage des navires manqués
 - Gestion des utilisateurs (Inscription, login, logout avec Auth0)
 - Leaderbord
 - Revenir un coups avant

## Liste des fonctionnalités du cours complétées :

 - Historiser les batailles (pouvoir revenir en arrière)
 - Ajouter un leaderboard et d’autres informations visuelle (« porte-avion coulé »)
 - Faire en sorte de pouvoir recommencer une partie
 - Côté front : ajouter des images pour les bateaux plutôt que des lettres (nous avons choisis un style minimaliste avec des couleurs plutôt que des images)
 - Permettre au joueur de placer ses bateaux à sa façon
 - Ajouter de la sécurité sur l’application pour sécuriser les échanges et ajouter une gestion d’utilisateurs
 - Améliorer le comportement de l’IA (attaque randomisée par périmètre)
 - Rajouter un niveau de difficulté (taille de la grille & intelligence IA)

## Fonctionnement de notre application
Au lancement de notre application, une page de login invite l'utilisateur à s'identifier. Si celui-ci ne possède pas de compte, il pourra alors en créé un.
Une fois cela fait, l'utilisateur a accès à notre bataille navale.
Avant de commencer à jouer, l'utilisateur peut choisir le niveau de difficulté de l'IA parmi 3 niveaux : facile, moyen, difficile. Pour augmenter encore le niveau de difficulté, l'utilisateur peut aussi choisir la taille de la grille qui peut aller de 6x6 à 20x20. Quelle que soit la taille de la grille, le nombre de bateau ne change pas.

Une fois ces paramètres validés, l'utilisateur peut ensuite placer les bateaux comme il le souhaite en les glissant-déposant. Par défaut les bateaux sont placés sur la grille mais tant que la grille n'est pas validée, la partie n'a pas commencé. L'utilisateur peut donc déplacer les bateaux ainsi que les tourner à l'aide de la touche "CTRL" de son clavier.
Lorsque les bateaux sont correctement placés, l'utilisateur valide la grille et la partie peut commencer.

Pour jouer, l'utilisateur clique sur la grille adverse afin de trouver les bateaux de l'IA. L'utilisateur a aussi la possibilité d'annuler son coup ou de recommencer la partie.

Pour plus de compétition, un leaderboard a été mis en place dans notre application. Tous les utilisateurs ayant un compte sur notre application sont recensés dans le leaderbord avec un nombre de points. Une partie gagnée rapport la taille de la grille * 10 points au joueur et ainsi, un classement des différents joueurs est proposés en cliquant sur le symbole de l'étoile à côté du nom d'utilisateur.

L'utilisateur peut se déconnecter dans le menu en haut a gauche.
