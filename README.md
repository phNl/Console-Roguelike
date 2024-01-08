# Console Rogue Like
### Игра в жанре рогалик/данжен кроулер.
Имеет:
- Cлучайно генерирующийся лабиринт-карту;
- Передвигающегося персонажа;
- Двух типов противника, который атакует вблизи и стреляет снарядами.
- Выход/переход на следующий уровень (бесконечная структура).
- Риалтайм система вместо пошаговой
- Возможность атаковать противников.

### Код
Основной процесс игры написан в скрипте GameController.cs в папке Game

### Обозначения
- \# - стена
- O - игрок
- R, M - враги, стрелок и ближник соответственно (Range, Melee)
- E - переход на следующий уровень

### Управление
Движение WASD, Атака на стрелки, Выход из игры на Escape

### Запуск
Только под Windows (небольшая часть кода написана для её консоли). 
Делал с этой настройкой (на скриншоте), скорее всего без нее работать не будет (хотя можно попробовать)
(ПКМ по шапке окна консоли -> Свойства -> Настройки)

![изображение](https://github.com/phNl/Console-Roguelike/assets/86802257/70e6be37-eff8-4d3b-a900-cf709e87aa73)

Также игра меняет настройку размера буфера консоли, из-за чего в нее будет помещаться ограниченное число строк/столбцов.
Она находится в тех же свойствах (на скриншоте ниже), на тот случай, если надо будет их вернуть на бОльшие значения.

![изображение](https://github.com/phNl/Console-Roguelike/assets/86802257/7c10515f-9543-4672-ab25-6d15d243b109)
