# BashInt
An Interpreter for Bash - Coded entirely in C# - compatible with .Net framework 2.0+

### Checklist:
- [x] Functions
- [ ] Variables
- [ ] Modular Structure

### SH/BASH Components Checklist:

- [x] Sleep
- [ ] printf/ echo -e
 - [x] Colour (\e0m, \e[38;5;217m....)
 - [ ] Clear (\ec)
 - [x] Resizing (\e[8;10;80t)
 - [ ] Cursor Movement
       - [ ] Clear (\033[2J)
       - [ ] Clear to EOL (\033[K)
       - [ ] Save (\033[s)
       - [ ] Restore (\033[u)
       - [ ] Forward (\033[<N>C)
       - [ ] Backward (\033[<N>B)
       - [ ] Position (\033[<L>;<C>f)
 - [x] Accept different all escape variants
    - [x] \e
    - [x] \033
  
