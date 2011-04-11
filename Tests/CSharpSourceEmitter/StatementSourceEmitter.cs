﻿//-----------------------------------------------------------------------------
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the Microsoft Public License.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Cci;

namespace CSharpSourceEmitter {
  public partial class SourceEmitter : CodeTraverser, ICSharpSourceEmitter {

    public override void TraverseChildren(IBlockStatement block) {
      PrintToken(CSharpToken.LeftCurly);
      base.TraverseChildren(block);
      PrintToken(CSharpToken.RightCurly);
    }

    public override void TraverseChildren(IAssertStatement assertStatement) {
      this.PrintToken(CSharpToken.Indent);
      sourceEmitterOutput.Write("CodeContract.Assert(");
      this.Traverse(assertStatement.Condition);
      if (assertStatement.Description != null) {
        sourceEmitterOutput.Write(",");
        this.Traverse(assertStatement.Description);
      }
      sourceEmitterOutput.WriteLine(");");
    }

    public override void TraverseChildren(IAssumeStatement assumeStatement) {
      this.PrintToken(CSharpToken.Indent);
      sourceEmitterOutput.Write("CodeContract.Assume(");
      this.Traverse(assumeStatement.Condition);
      if (assumeStatement.Description != null) {
        sourceEmitterOutput.Write(",");
        this.Traverse(assumeStatement.Description);
      }
      sourceEmitterOutput.WriteLine(");");
    }

    public override void TraverseChildren(IBreakStatement breakStatement) {
      this.PrintToken(CSharpToken.Indent);
      sourceEmitterOutput.WriteLine("break;");
    }

    public override void TraverseChildren(IConditionalStatement conditionalStatement) {
      sourceEmitterOutput.Write("if (", true);
      this.Traverse(conditionalStatement.Condition);
      sourceEmitterOutput.Write(")");
      if (conditionalStatement.TrueBranch is IBlockStatement)
        this.Traverse(conditionalStatement.TrueBranch);
      else {
        PrintToken(CSharpToken.NewLine);
        sourceEmitterOutput.IncreaseIndent();
        this.Traverse(conditionalStatement.TrueBranch);
        sourceEmitterOutput.DecreaseIndent();
      }
      if (!(conditionalStatement.FalseBranch is IEmptyStatement)) {
        this.sourceEmitterOutput.Write("else", true);
        this.Traverse(conditionalStatement.FalseBranch);
      }
    }

    public override void TraverseChildren(IContinueStatement continueStatement) {
      sourceEmitterOutput.WriteLine("continue;");
    }

    public override void TraverseChildren(ICatchClause catchClause) {
      base.TraverseChildren(catchClause);
    }

    public override void TraverseChildren(IDebuggerBreakStatement debuggerBreakStatement) {
      this.PrintToken(CSharpToken.Indent);
      sourceEmitterOutput.WriteLine("Debugger.Break();");
    }

    public override void TraverseChildren(IDoUntilStatement doUntilStatement) {
      base.TraverseChildren(doUntilStatement);
    }

    public override void TraverseChildren(IEmptyStatement emptyStatement) {
      base.TraverseChildren(emptyStatement);
    }

    public override void TraverseChildren(IExpressionStatement expressionStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.Traverse(expressionStatement.Expression);
      this.PrintToken(CSharpToken.Semicolon);
    }

    public override void TraverseChildren(IFieldReference fieldReference) {
      this.sourceEmitterOutput.Write(MemberHelper.GetMemberSignature(fieldReference, NameFormattingOptions.None));
    }

    public override void TraverseChildren(IFileReference fileReference) {
      base.TraverseChildren(fileReference);
    }

    public override void TraverseChildren(IForEachStatement forEachStatement) {
      base.TraverseChildren(forEachStatement);
    }

    public override void TraverseChildren(IForStatement forStatement) {
      base.TraverseChildren(forStatement);
    }

    public override void TraverseChildren(IFunctionPointerTypeReference functionPointerTypeReference) {
      base.TraverseChildren(functionPointerTypeReference);
    }

    public override void TraverseChildren(IGenericMethodInstanceReference genericMethodInstanceReference) {
      base.TraverseChildren(genericMethodInstanceReference);
    }

    public override void TraverseChildren(IGenericMethodParameterReference genericMethodParameterReference) {
      base.TraverseChildren(genericMethodParameterReference);
    }

    public override void TraverseChildren(IGenericTypeInstanceReference genericTypeInstanceReference) {
      base.TraverseChildren(genericTypeInstanceReference);
    }

    public override void TraverseChildren(IGenericTypeParameterReference genericTypeParameterReference) {
      base.TraverseChildren(genericTypeParameterReference);
    }

    public override void TraverseChildren(IGotoStatement gotoStatement) {
      this.sourceEmitterOutput.Write("goto ", true);
      this.sourceEmitterOutput.Write(gotoStatement.TargetStatement.Label.Value);
      this.sourceEmitterOutput.WriteLine(";");
    }

    public override void TraverseChildren(IGotoSwitchCaseStatement gotoSwitchCaseStatement) {
      base.TraverseChildren(gotoSwitchCaseStatement);
    }

    public override void TraverseChildren(ILabeledStatement labeledStatement) {
      this.sourceEmitterOutput.DecreaseIndent();
      this.sourceEmitterOutput.Write(labeledStatement.Label.Value, true);
      this.sourceEmitterOutput.WriteLine(":");
      this.sourceEmitterOutput.IncreaseIndent();
      this.Traverse(labeledStatement.Statement);
    }

    public override void TraverseChildren(ILocalDefinition localDefinition) {
      base.TraverseChildren(localDefinition);
    }

    public override void TraverseChildren(ILocalDeclarationStatement localDeclarationStatement) {
      string type = TypeHelper.GetTypeName(localDeclarationStatement.LocalVariable.Type, NameFormattingOptions.ContractNullable|NameFormattingOptions.UseTypeKeywords);
      this.sourceEmitterOutput.Write(type, true);
      this.sourceEmitterOutput.Write(" ");
      this.PrintLocalName(localDeclarationStatement.LocalVariable);
      if (localDeclarationStatement.InitialValue != null) {
        this.sourceEmitterOutput.Write(" = ");
        this.Traverse(localDeclarationStatement.InitialValue);
      }
      this.sourceEmitterOutput.WriteLine(";");
    }

    public override void TraverseChildren(ILockStatement lockStatement) {
      base.TraverseChildren(lockStatement);
    }

    public override void TraverseChildren(IPushStatement pushStatement) {
      this.sourceEmitterOutput.Write("push ", true);
      this.Traverse(pushStatement.ValueToPush);
      this.sourceEmitterOutput.WriteLine(";");
    }

    public override void TraverseChildren(IResourceUseStatement resourceUseStatement) {
      base.TraverseChildren(resourceUseStatement);
    }

    public override void TraverseChildren(IRethrowStatement rethrowStatement) {
      this.sourceEmitterOutput.WriteLine("throw;", true);
    }

    public override void TraverseChildren(IReturnStatement returnStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.PrintToken(CSharpToken.Return);
      if (returnStatement.Expression != null) {
        this.PrintToken(CSharpToken.Space);
        this.Traverse(returnStatement.Expression);
      }
      this.PrintToken(CSharpToken.Semicolon);
      this.PrintToken(CSharpToken.NewLine);
    }

    public override void TraverseChildren(IStatement statement) {
      base.TraverseChildren(statement);
    }

    public override void TraverseChildren(ISwitchCase switchCase) {
      if (switchCase.IsDefault)
        this.sourceEmitterOutput.WriteLine("default:", true);
      else {
        this.sourceEmitterOutput.Write("case ", true);
        this.Traverse(switchCase.Expression);
        this.sourceEmitterOutput.WriteLine(":");
      }
      this.sourceEmitterOutput.IncreaseIndent();
      this.Traverse(switchCase.Body);
      this.sourceEmitterOutput.DecreaseIndent();
    }

    public override void TraverseChildren(ISwitchStatement switchStatement) {
      this.sourceEmitterOutput.Write("switch(", true);
      this.Traverse(switchStatement.Expression);
      this.sourceEmitterOutput.WriteLine("){");
      this.sourceEmitterOutput.IncreaseIndent();
      this.Traverse(switchStatement.Cases);
      this.sourceEmitterOutput.DecreaseIndent();
      
      this.sourceEmitterOutput.WriteLine("}", true);
    }

    public override void TraverseChildren(IThrowStatement throwStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.PrintToken(CSharpToken.Throw);
      if (throwStatement.Exception != null) {
        this.PrintToken(CSharpToken.Space);
        this.Traverse(throwStatement.Exception);
      }
      this.PrintToken(CSharpToken.Semicolon);
    }

    public override void TraverseChildren(ITryCatchFinallyStatement tryCatchFilterFinallyStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.PrintToken(CSharpToken.Try);
      this.Traverse(tryCatchFilterFinallyStatement.TryBody);
      foreach (ICatchClause clause in tryCatchFilterFinallyStatement.CatchClauses) {
        this.sourceEmitterOutput.Write("catch", true);
        if (clause.ExceptionType != Dummy.TypeReference) {
          this.sourceEmitterOutput.Write("(");
          this.PrintTypeReference(clause.ExceptionType);
          if (clause.ExceptionContainer != Dummy.LocalVariable) {
            this.sourceEmitterOutput.Write(" ");
            this.PrintLocalName(clause.ExceptionContainer);
          }
          this.sourceEmitterOutput.Write(")");
        }
        if (clause.FilterCondition != null) {
          this.sourceEmitterOutput.WriteLine("{");
          this.sourceEmitterOutput.IncreaseIndent();
          this.sourceEmitterOutput.Write("if (", true);
          this.Traverse(clause.FilterCondition);
          this.sourceEmitterOutput.WriteLine(" == 1)");
          this.Traverse(clause.Body);
          this.sourceEmitterOutput.DecreaseIndent();
          this.sourceEmitterOutput.WriteLine("}", true);
        }
        this.Traverse(clause.Body);
      }
      if (tryCatchFilterFinallyStatement.FaultBody != null) {
        this.sourceEmitterOutput.Write("fault", true);
        this.Traverse(tryCatchFilterFinallyStatement.FaultBody);
      }
      if (tryCatchFilterFinallyStatement.FinallyBody != null) {
        this.sourceEmitterOutput.Write("finally", true);
        this.Traverse(tryCatchFilterFinallyStatement.FinallyBody);
      }
    }

    public override void TraverseChildren(IWhileDoStatement whileDoStatement) {
      base.TraverseChildren(whileDoStatement);
    }

    public override void TraverseChildren(IWin32Resource win32Resource) {
      base.TraverseChildren(win32Resource);
    }

    public override void TraverseChildren(IYieldBreakStatement yieldBreakStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.PrintToken(CSharpToken.YieldBreak);
      this.PrintToken(CSharpToken.Semicolon);
      this.PrintToken(CSharpToken.NewLine);
    }

    public override void TraverseChildren(IYieldReturnStatement yieldReturnStatement) {
      this.PrintToken(CSharpToken.Indent);
      this.PrintToken(CSharpToken.YieldReturn);
      if (yieldReturnStatement.Expression != null) {
        this.PrintToken(CSharpToken.Space);
        this.Traverse(yieldReturnStatement.Expression);
      }
      this.PrintToken(CSharpToken.Semicolon);
      this.PrintToken(CSharpToken.NewLine);
    }

  }
}