prequire("common.lua");


-- �O���[�o���ϐ�
V = {}
V.state = "init" -- �ʐM�̏��



function OnLoad()
	PH = {};	-- �v���Z�X�Ǘ��e�[�u��(Process Handle)
end


function boot_OnStart()
	require 'StdProxyAIMS' -- �V�X�e���̃��[�h

	-- StdProxyAIMS.StartProcess �� �Ԃ�l �� �v���Z�X���ʃn���h��
	-- ��������������������������������������������������������������������������������
	-- ���{���͂��̃n���h�����g�����Ƃɂ��A�����ɂ������̃v���Z�X�ƒʐM�ł��܂��B��
	-- ���@�������A�����_�ł͂P�̃v���Z�X�̋N���ɂ����Ή����Ă��܂���B�@�@�@�@�@�@��
	-- ���@�������A�v���Ƃ̓����ʐM�͂ł��܂���B�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��
	-- ��������������������������������������������������������������������������������

    -- �v���Z�X��STD�ʐM���[�h�N�����A�ϐ��uPH.Handle�v�Ƀn���h����ۑ�����B
	PH.Sample = StdProxyAIMS.StartProcess("./GameServer/GameServer/bin/Release/Sample.GameServer.exe")


	--������AIMS�ŗL
	font1 = createTextFont("Meiryo", 32)
	text1 = createBlankGraphic( 640, 64)
	createActor( text1, 320, 32, 11 )
	-- �����܂�

end

--
-- ���邭�����Ă��遙��
--
function boot_OnStep()

    if V.state == "init" then

        -- �N�������v���Z�X�ɁA������𑗂�܂��B�iSTDIN�ɕ����񑗐M�j�����̉��s�͕K�v�B
	    StdProxyAIMS.WriteData(PH.Sample, "���������N���끙����\n");
        V.state = "waitAwake"

    elseif V.state == "waitAwake" then

        -- �N�������v���Z�X���瑗���Ă��镶������Ď����܂��B�iSTDOUT�̕�������擾�j
        receive = StdProxyAIMS.ReadData(PH.Sample);
        
        if receive then -- ��M���ꂽ�����񂪂Ȃ��ꍇ�� nil �ɂȂ�̂Œe���遙

            --������AIMS�ŗL
            fillGraphic(text1, 0, 0, 25, 255);
            drawTextToGraphic( text1, font1, 0, 0, receive, 255, 255, 255, 255, false)
            -- �����܂�

        end

    end

    --sleep(300) -- ��u�ŕ���������Ă��܂��̂Œx�����Ă݂�i���^�p�ł͖��Ӗ��j

end

function boot_OnClose()
end

function OnVanish()
	StdProxyAIMS.CloseProcess(PH.Sample);
end

