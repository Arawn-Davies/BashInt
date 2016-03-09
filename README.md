# BashInt
An Interpreter for Bash - Coded entirely in C# - compatible with .Net framework 2.0+
![BashInt](http://i.snag.gy/s47Bt.jpg)

### Checklist:
- [x] Functions
 - [x] Functions Selector and Parser
 - [x] Function Calls
 - [ ] Argument Passing
- [ ] Variables
 - [x] Variable Parsing 
 - [ ] Indexing
- [ ] Modular Structure
 - [ ] Struct
 - [x] dll parsing
- [x] Display
 - [ ] printf/echo -e Commands (see below) 
 - [x] Print System: Rich TextBox
 - [ ] More efficient printing - own component (postponded due to the lesser overhead of current system)
- [x] Debugger
 - [x] Syntax Highlighting
 - [x] Current LOC display
 - [x] Automatic Scrolling
 - [x] Estimate % execution bar

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
 - [x] Escaping 
   - [x] Accept different all escape variants (unicode, ascii, etc...)
    - [x] \e
    - [x] \033
  
