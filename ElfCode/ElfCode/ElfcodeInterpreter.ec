#ip 0
// register 0 = IP
// register 1 - 6 program registers
// register 7 bit ipbound
// register 8 ipBinding
// register 9 general jumpback
// register 10 instruction list pointer
// register 11 instruction list count
// 12 - 14 reserved
// 15 - 20 working memory
// 21 - 420 instruction list (space for 100 instructions)
// exit codes: 10000 = parse error
//
// 0
addi 0 0 0
// 1 get next input line
peek 0 0 15
addr 0 15 0
seti 8999 0 0		// go to program execution TODO
geti 0 0 16
// 5 check for #ip
eqri 16 35 15
addr 0 15 0
seti 17 0 0			// not #ip, go to regular line parsing
geti 0 0 16
geti 0 0 16
geti 0 0 16
geti 0 0 16
bani 16 15 16
seti 1 0 7
setr 16 0 8 
geti 0 0 16
geti 0 0 16
seti 0 0 0
// 18 parse instruction, split on first char
geti 0 0 17
geti 0 0 18
geti 0 0 19
eqri 16 97 15
eqri 15 0 15
addr 0 15 0
seti 53 0 0			// jump to a___ parsing
eqri 16 98 15
eqri 15 0 15
addr 0 15 0
seti 67 0 0			// jump to b___ parsing
eqri 16 101 15
eqri 15 0 15
addr 0 15 0
seti 95 0 0			// jump to e___ parsing
eqri 16 103 15
eqri 15 0 15
addr 0 15 0
seti 116 0 0		// jump to g___ parsing
eqri 16 109 15
eqri 15 0 15
addr 0 15 0
seti 147 0 0		// jump to m___ parsing
eqri 16 111 15
eqri 15 0 15
addr 0 15 0
seti 161 0 0		// jump to o___ parsing
eqri 16 112 15
eqri 15 0 15
addr 0 15 0
seti 175 0 0		// jump to p___ parsing 
eqri 16 115 15
eqri 15 0 15
addr 0 15 0
seti 185 0 0		// jump to s___ parsing
seti 9999 0 0
// resolve individual instructions
// 54 a___ parsing
eqri 17 100 15		// ad
addr 0 15 0
seti 9999 0 0
eqri 18 100 15		// add
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// addi
eqri 15 0 15
addr 0 15 0
seti 199 0 0		// jump to addi parsing
eqri 19 114 15		// addr
addr 0 15 0
seti 9999 0 0
seti 201 0 0		// jump to addr parsing TODO
// 68 b___ parsing
eqri 17 97 15		// ba
addr 0 15 0
addi 0 11 0
eqri 18 110 15		// ban
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// bani
eqri 15 0 15
addr 0 15 0
seti 203 0 0		// jump to bani parsing TODO
eqri 19 114 15		// banr
addr 0 15 0
seti 9999 0 0
seti 205 0 0		// jump to banr parsing TODO
// 82 borr and bori
eqri 17 111 15		// bo
addr 0 15 0
seti 9999 0 0
eqri 18 114 15		// bor
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// bori
eqri 15 0 15
addr 0 15 0
seti 207 0 0		// jump to bori parsing TODO
eqri 19 114 15		// borr
addr 0 15 0
seti 9999 0 0
seti 209 0 0		// jump to borr parsing TODO
// 96 e___ parsing
eqri 17 113 15		// eq
addr 0 15 0
seti 9999 0 0
eqri 18 105 15		// eqi
addr 0 15 0
addi 0 4 0
eqri 19 114 15		// eqir
addr 0 15 0
seti 9999 0 0
seti 211 0 0		// jump to eqir parsing TODO
// 106 eqri and eqrr
eqri 18 114 15		// eqr
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// eqri
eqri 15 0 15
addr 0 15 0
seti 213 0 0		// jump to eqri parsing TODO
eqri 19 114 15		// eqrr
addr 0 15 0
seti 9999 0 0
seti 215 0 0		// jump to eqrr parsing TODO
// 117 g___ parsing
eqri 17 101 15		// ge
addr 0 15 0
addi 0 7 0
eqri 18 116 14		// get
addr 0 15 0
seti 9999 0 0
eqri 18 105 15		// geti
addr 0 15 0
seti 217 0 0		// jump to geti parsing TODO
seti 9999 0 0
eqri 17 116 15		// gt
addr 0 15 0
seti 9999 0 0
eqri 18 105 15		// gti
addr 0 15 0
addi 0 4 0
eqri 19 114 15		// gtir
addr 0 15 0
seti 9999 0 0
seti 219 0 0		// jump to gtir parsing TODO
eqri 18 114 15		// gtr
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// gtri
eqri 15 0 15
addr 0 15 0
seti 221 0 0		// jump to gtri parsing TODO
eqri 19 114 15		// gtrr
addr 0 15 0
seti 9999 0 0
seti 223 0 0		// jump to gtrr parsing TODO
//148 m___ parsing
eqri 17 117 15		// mu
addr 0 15 0
seti 9999 0 0
eqri 18 108 15		// mul
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// muli
eqri 15 0 15
addr 0 15 0
seti 225 0 0		// jump to muli parsing TODO
eqri 19 114 15		// mulrr
addr 0 15 0
seti 9999 0 0
seti 227 0 0 		// jump to mulr parsing TODO
// 162 o___ parsing
eqri 17 117 15		// ou
addr 0 15 0
seti 9999 0 0
eqri 18 116 15		// out
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// outi
eqri 15 0 15
addr 0 15 0
seti 229 0 0		// jump to outi parsing TODO
eqri 19 114 15		// outr
addr 0 15 0
seti 9999 0 0
seti 231 0 0		// jump to outr parsing TODO
// 176 p___ parsing
eqri 17 101 15  	// pe
addr 0 15 0
seti 9999 0 0
eqri 18 101 15		// pee
addr 0 15 0
seti 9999 0 0
eqri 19 107 15		// peek
addr 0 15 0
seti 9999 0 0
seti 233 0 0 		// jump to peek parsing TODO
// 186 s___ parsing
eqri 17 101 15		// se
addr 0 15 0
seti 9999 0 0
eqri 18 116 15		// set
addr 0 15 0
seti 9999 0 0
eqri 19 105 15		// seti
eqri 15 0 15
addr 0 15 0
seti 235 0 0		// jump to seti parsing TODO
eqri 19 114 15		// setr
addr 0 15 0
seti 9999 0 0
seti 237 0 0		// jump to setr parsing TODO
// 200
// parse parameters of the different operators
// addi
seti 0 0 20			
seti 239 0 0		// jump to parameter parsing
// addr
seti 1 0 20
seti 239 0 0		// jump to parameter parsing
// bani
seti 2 0 20
seti 239 0 0		// jump to parameter parsing
// banr
seti 3 0 20
seti 239 0 0		// jump to parameter parsing
// bori
seti 4 0 20
seti 239 0 0		// jump to parameter parsing
// borr
seti 5 0 20
seti 239 0 0		// jump to parameter parsing
// eqir
seti 6 0 20
seti 239 0 0		// jump to parameter parsing
// eqri
seti 7 0 20
seti 239 0 0		// jump to parameter parsing
// eqrr
seti 8 0 20
seti 239 0 0		// jump to parameter parsing
// geti
seti 9 0 20
seti 239 0 0		// jump to parameter parsing
// gtir
seti 10 0 20
seti 239 0 0		// jump to parameter parsing
// gtri
seti 11 0 20
seti 239 0 0		// jump to parameter parsing
// gtrr
seti 12 0 20
seti 239 0 0		// jump to parameter parsing
// muli
seti 13 0 20
seti 239 0 0		// jump to parameter parsing
// mulr
seti 14 0 20
seti 239 0 0		// jump to parameter parsing
// outi
seti 15 0 20
seti 239 0 0		// jump to parameter parsing
// outr
seti 16 0 20
seti 239 0 0		// jump to parameter parsing
// peek
seti 17 0 20
seti 239 0 0		// jump to parameter parsing
// seti
seti 18 0 20
seti 239 0 0		// jump to parameter parsing
// setr
seti 19 0 20
seti 239 0 0		// jump to parameter parsing
// 239 parameter parsing
seti 0 0 17
geti 0 0 16
geti 0 0 16
bani 16 15 16		// parse a number
addr 16 17 17
geti 0 0 16
eqri 16 32 15		// is a space?
muli 15 2 15		// skip to second parameter
addr 0 15 0
muli 17 10 17
seti 242 0 0
// second parameter
seti 0 0 18
geti 0 0 16
bani 16 15 16
addr 16 18 18
geti 0 0 16
eqri 16 32 15
muli 15 2 15
addr 0 15 0
muli 18 10 18
seti 252 0 0
// third parameter
seti 0 0 19
geti 0 0 16
bani 16 15 16
addr 16 19 19
peek 0 0 15			// are we out of input?
eqri 15 0 15
muli 15 9 15
addr 0 15 0
geti 0 0 16
eqri 16 13 15		// is a CR?
muli 15 5 15
addr 0 15 0
eqri 16 32 15		// is a space?
muli 15 2 15
addr 0 15 0
muli 19 10 19
seti 262 0 0
// 278 loop until input is a linefeed or we're out of file
peek 0 0 15
addr 0 15 0
seti 499 0 0 B
geti 0 0 16
eqri 16 10 15
addr 0 15 0
seti 277 0 0
seti 499 0 0 B
// reserved space
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0
addi 0 0 0 
// 500 instruction list
// Add instruction:
muli 11 5 16 B
addi 16 1 16
addi 11 1 11
setr 0 0 9
addi 9 2 9
addr 0 16 0
seti 0 0 0
//
setr 20 0 21
setr 17 0 22
setr 18 0 23
setr 19 0 24
setr 9 0 0
setr 20 0 25
setr 17 0 26
setr 18 0 27
setr 19 0 28
setr 9 0 0