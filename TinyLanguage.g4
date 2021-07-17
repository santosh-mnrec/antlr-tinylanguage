grammar TinyLanguage;

program: codeBlock EOF;

codeBlock: statement*;

statement: assignment | if_stat | print;

assignment: VAR ID ASSIGN expr SCOL;

if_stat:
	IF condition_block (ELSE IF condition_block)* (
		ELSE stat_block
	)?;

condition_block: expr OBRACE stat_block CBRACE;

stat_block: OBRACE codeBlock CBRACE | statement;

print: PRINT OPAR expr CPAR SCOL;

expr:
	expr op = (PLUS | MINUS) expr	# additiveExpr
	| expr op = (EQ | NEQ) expr		# equalityExpr
	| expr AND expr					# andExpr
	| expr OR expr					# orExpr
	| atom							# atomExpr;

atom:
	OPAR expr CPAR	# parExpr
	| (INT | FLOAT)	# numberAtom
	| ID			# idAtom
	| STRING		# stringAtom;

OR: '||';
AND: '&&';
EQ: '==';
NEQ: '!=';
PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';
SCOL: ';';
ASSIGN: '=';
OPAR: '(';
CPAR: ')';
OBRACE: '{';
CBRACE: '}';
IF: 'if';
ELSE: 'else';
PRINT: 'print';
VAR: 'var';

ID: [a-zA-Z_] [a-zA-Z_0-9]*;

INT: [0-9]+;

FLOAT: [0-9]+ '.' [0-9]* | '.' [0-9]+;

STRING: '"' (~["\r\n] | '""')* '"';

COMMENT: '////' ~[\r\n]* -> skip;

SPACE: [ \t\r\n] -> skip;
