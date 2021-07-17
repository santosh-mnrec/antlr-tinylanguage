using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using static TinyLanguageParser;

namespace TinyLanguage
{
    public class Utils
    {

        public static void AddToDictionary(Dictionary<string, Variable> dict, string key, Variable value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key] = value;
            }
        }
    }
    public class TinyLanguageVisitor : TinyLanguageBaseVisitor<Variable>
    {

        private Dictionary<string, Variable> symbolsTable = new Dictionary<string, Variable>();

        public override Variable VisitAssignment([NotNull] AssignmentContext context)
        {
            var id = context.ID().GetText();
            var value = Visit(context.expr());
            Utils.AddToDictionary(symbolsTable, id, value);
            return value;

        }

        public override Variable VisitIdAtom([NotNull] IdAtomContext context)
        {
            var id = context.GetText();
            if (!symbolsTable.ContainsKey(id))
            {
                throw new Exception($"Symbol {id} is not defined");
            }
            return symbolsTable[id];
        }
        public override Variable VisitStringAtom([NotNull] StringAtomContext context)
        {
            var text = context.GetText();

            text = text.Substring(1, text.Length - 1).Replace("\\(.)", "$1").Replace("\"", "");

            return new Variable(text);

        }
        public override Variable VisitNumberAtom([NotNull] NumberAtomContext context)
        {
            return new Variable(int.Parse(context.GetText()));
        }
        public override Variable VisitParExpr([NotNull] ParExprContext context)
        {
            return this.Visit(context.expr());
        }
        public override Variable VisitAdditiveExpr([NotNull] AdditiveExprContext context)
        {
            if (context.ChildCount==1)
            {
                return Visit(context.expr()[0]);
            }
            var left = this.Visit(context.expr(0));
            var right = this.Visit(context.expr(1));
            switch (context.op.Type)
            {
                case (int)TinyLanguageParser.PLUS:
                    return AddHelper(left, right);
                case (int)TinyLanguageParser.MINUS:

                    return new Variable(left.ToInteger() - right.ToInteger());
                default:
                    throw new Exception("Unknown operator");


            }

        }
        private Variable AddHelper(Variable left, Variable right){
            if (left.IsNumber() && right.IsNumber()){
                return new Variable(left.ToInteger() + right.ToInteger());
            }else if(left.IsString() || right.IsString()){
                return new Variable(left.ToString() + right.ToString());
            }
            throw new Exception("Cannot add " + left.ToString() + " and " + right.ToString());

        }
        public override Variable VisitEqualityExpr([NotNull] EqualityExprContext context)
        {
            var left = this.Visit(context.expr(0));
            var right = this.Visit(context.expr(1));
            switch (context.op.Type)
            {
                case (int)TinyLanguageParser.EQ:
                    return new Variable(left.ToInteger() == right.ToInteger());
                case (int)TinyLanguageParser.NEQ:
                    return new Variable(left.ToInteger() != right.ToInteger());
                default:
                    throw new Exception("Unknown operator");

            }


        }
        public override Variable VisitOrExpr([NotNull] OrExprContext context)
        {
            var left = this.Visit(context.expr(0));
            var right = this.Visit(context.expr(1));
            var result = new Variable(left.ToBoolean() || right.ToBoolean());
            return result;
        }
        public override Variable VisitAndExpr([NotNull] AndExprContext context)
        {
            var left = this.Visit(context.expr(0));
            var right = this.Visit(context.expr(1));
            var result = new Variable(left.ToBoolean() && right.ToBoolean());
            return result;
        }
        public override Variable VisitIf_stat([NotNull] If_statContext context)
        {
            var blockExecuted = false;
            var conditions = context.condition_block();


            foreach (var condition in conditions)
            {
                var conditionValue = this.Visit(condition.expr());
                if (conditionValue.ToBoolean())
                {
                    blockExecuted = true;
                    var block = this.Visit(condition.stat_block());
                    break;
                }


            }
            if (!blockExecuted && context.stat_block() != null)
            {
                // evaluate the else-stat_block (if present == not null)
                this.Visit(context.stat_block());
            }

            return Variable.VOID;
        }
        public override Variable VisitPrint([NotNull] PrintContext context)
        {
            var value = this.Visit(context.expr());
            System.Console.WriteLine(value.ToString());
            return value;
        }

    }
}