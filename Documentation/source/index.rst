.. figure:: _static/index/cover.gif
    :align: center
    :width: 100%

******************************
Elinging on Time Documentation
******************************

Elinging On Time was the birthday gift I have prepared for my girlfriend Elina Liu, the name Elinging is a nickname I created for her avatar in the game. The game is based on an occusion where Elinging has an important seminar in the morning, however she got up late, thus needed to run like hell in order to catch up with the meeting. Since Elina and I were both Imperial College students, the game has been taken place in London as you will soon spot that there are many iconic elements in the game such as the double decker bus and red telephone booth.

.. figure:: _static/index/elina.png
    :width: 300
    :align: center

    The lovely girlfreind Elina Liu

Game Features
-------------

.. _Sample Play: https://www.youtube.com/watch?v=JBx09f0pFZw

* Please see the `Sample Play`_ on YouTube to see the basic game features.

.. figure:: _static/index/everyone.png
    :align: center

    Elingling On Time follows a pixel art style

Basic Control
~~~~~~~~~~~~~

:WebGL:
    The WebGL version of the game follows the most basic simple control of using :guilabel:`w` and :guilabel:`s` to control the avatar to move up and down.

:Phone:
    The Phone version of the game utilise the accelerometer of the phone, when the phone has been tilting over a certain degree, the player will go towards the up direction and vice versa.

Game Elements
~~~~~~~~~~~~~~

:Vehicle:
    Two kinds of vehcles are on the road in the game, the beetles and the double decker bus. When the player avatar crash with the vehicle, one health point will be deducted.

:Soldier:
    Due that Elinging violate the traffic rules, soldiers and on the road will start chasing down Elinging during the running, the speed of soldier has been set to slightly faster than the running speed of Elingling thus Elingling need to ride a bicycle in order to escape away from the soldiers.

:Bicycle:
    The Santander bicycle grants Elinging 3 seconds of speed buff. In addition, Elinging enters the invincible mode when riding the bicycle where she can bascially knock away the vehcles on the road.

********
Contents
********

.. toctree::
    :maxdepth: 2
    :caption: Game Architecture:

    arch/event_handling.rst
    arch/config_data.rst
    arch/menu.rst

.. toctree::
    :maxdepth: 2
    :caption: Gameplay Functionality Implementation:

    func/player.rst
    func/interactive_objects.rst
    func/env_loop.rst



Indices and tables
------------------

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`
