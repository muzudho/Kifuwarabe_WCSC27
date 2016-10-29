﻿using Grayscale.A210_KnowNingen_.B280_Tree_______.C___500_Struct;
using Grayscale.A210_KnowNingen_.B270_Sky________.C___500_Struct;
using Grayscale.A210_KnowNingen_.B240_Move_______.C___500_Struct;

namespace Grayscale.A210_KnowNingen_.B740_KifuParserA.C___500_Parser
{
    public interface KifuParserA_Result
    {
        Move Out_newMove_OrNull { get; }

        Sky NewSky { get; }

        void SetNode(Move node, Sky sky);
    }
}
