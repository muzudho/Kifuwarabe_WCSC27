prequire("common.lua");

function OnLoad()
	PH = {};	-- �v���Z�X�Ǘ��e�[�u��(Process Handle)
end

function boot_OnStart()
	require 'StdProxyAIMS'; -- �V�X�e���̃��[�h

	-- StdProxyAIMS.StartProcess �� �Ԃ�l �� �v���Z�X���ʃn���h��
	-- ��������������������������������������������������������������������������������
	-- ���{���͂��̃n���h�����g�����Ƃɂ��A�����ɂ������̃v���Z�X�ƒʐM�ł��܂��B��
	-- ���@�������A�����_�ł͂P�̃v���Z�X�̋N���ɂ����Ή����Ă��܂���B�@�@�@�@�@�@��
	-- ���@�������A�v���Ƃ̓����ʐM�͂ł��܂���B�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@�@��
	-- ��������������������������������������������������������������������������������

	PH.Sample = StdProxyAIMS.StartProcess("Sample.exe"); -- �v���Z�X��STD�ʐM���[�h�N�����A�ϐ��uPH.Handle�v�Ƀn���h����ۑ�����B�B

	--������AIMS�ŗL
	font1 = createTextFont("Meiryo", 32)
	text1 = createBlankGraphic( 640, 64);
	createActor( text1, 320, 32, 11 );
	-- �����܂�

	StdProxyAIMS.WriteData(PH.Sample, "This is Test Data!\n"); -- STDIN�ɕ����񑗐M
end

function boot_OnStep()
	R = StdProxyAIMS.ReadData(PH.Sample); -- STDOUT�̕�������擾
	if R then -- ��M���ꂽ�����񂪂Ȃ��ꍇ�� nil �ɂȂ�̂�

		--������AIMS�ŗL
		fillGraphic(text1, 0, 0, 25, 255);
		drawTextToGraphic( text1, font1, 0, 0, R, 255, 255, 255, 255, false)
		-- �����܂�

		sleep(300); -- ��u�ŕ���������Ă��܂��̂Œx�����Ă݂�i���^�p�ł͖��Ӗ��j
	end

end

function boot_OnClose()
end

function OnVanish()
	StdProxyAIMS.CloseProcess(PH.Sample);
end

