﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TPM;

namespace PCI_L122DSF_M1x1a
{
    public partial class Form1 : Form
    {
        short ret = 0;
        short ret2 = 0;
        short ret3 = 0;
        short ret4 = 0;
        short ret5 = 0;
        ushort m_Baudrate0 = 0;
        ushort m_CardNo = 0;

        ushort m_RingNo = 0;//only one ring        
        uint[] lDevTable = new uint[2];
        ///////////
        string[] IoStsName = new string[] {
            "RDY", "ALM", "+EL", "-EL", "ORG", "DIR", "EMG", "PCS",
            "ERC", "EZ", "CLR", "LTC", "SD", "INP", "SVON", "RALM"};
        Label[] lblIoSts = new Label[16];
        Label[] m_Label;
        Button[] btn_Reset;
        CheckBox[] cb_SvOn;

        Button[] btn_Move;
        Button[] btn_Move1;
        Button[] btn_Move2;
        Button[] btn_Move3;
        Button[] btn_Move4;
        Button[] btn_Move5;

        TextBox[] tb_Set;
        ComboBox[] cmb_Mode;
        ushort[] SlaveIP = new ushort[2];
        ushort[] SlaveIP1 = new ushort[2];
        ushort[] SlaveIP2 = new ushort[2];
        ushort[] SlaveIP3 = new ushort[2];
        ushort[] SlaveIP4 = new ushort[2];
        Label[] lb_sts = new Label[16];
        string path = string.Empty;

        public Form1()
        {
            InitializeComponent();
            m_Label = new Label[6] { label1, label2, label3, label4, label5, label6 };

            btn_Reset = new Button[1] { btnReset0 };
            cb_SvOn = new CheckBox[1] { ChkSVON0 };
            cmb_Mode = new ComboBox[6] { comboBox1, comboBox5, comboBox6, comboBox7, comboBox8, comboBox9 };

            btn_Move = new Button[3]  {  btnMoveP0,
                                         btnStop0,
                                         btnMoveM0,
                                      };
            btn_Move1 = new Button[3] { btnMoveP1,
                                        btnStop1,
                                        btnMoveM1
                                      };
            btn_Move2 = new Button[3] { btnMoveP2,
                                        btnStop2,
                                        btnMoveM2
                                      };
            btn_Move3 = new Button[3] { btnMoveP3,
                                        btnStop3,
                                        btnMoveM3
                                      };
            btn_Move4 = new Button[3] { btnMoveP4,
                                        btnStop4,
                                        btnMoveM4
                                      };
            btn_Move5 = new Button[3] { btnMoveP5,
                                        btnStop5,
                                        btnMoveM5
                                      };


            tb_Set = new TextBox[26] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14, textBox15, textBox16, textBox17, textBox18, textBox19, textBox20, textBox21, textBox22, textBox23, textBox24, textBox25,textBox26 };
            for (int i = 0; i < 6; i++)
            {
                if (i < 1)
                {
                    cmb_Mode[i].SelectedIndex = 0;
                    cmb_Mode[i].Tag = btn_Reset[i].Tag = cb_SvOn[i].Tag = btn_Move[i].Tag = btn_Move1[i].Tag = btn_Move2[i].Tag =  btn_Move3[i].Tag = btn_Move4[i].Tag = btn_Move5[i].Tag =  tb_Set[i].Tag = i;
                }
                else if (i >= 1 && i < 3)
                {
                    btn_Move[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                    btn_Move1[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                    btn_Move2[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                    btn_Move3[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                    btn_Move4[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                    btn_Move5[i].Tag = tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                }
                else if (i >= 3 && i < 4)
                    tb_Set[i].Tag = cmb_Mode[i].Tag = i;
                else
                    cmb_Mode[i].Tag = i;
            }

            //int x, y;
            ////for (int i = 0; i < 2; i++)
            //{
            //    x = grpIoSts.Width / 80;
            //    y = grpIoSts.Height / 8 + i * grpIoSts.Width/20;

            //    for (int j = 0; j < 16; j++)
            //    {
            //        if (j == 7 || j == 10 || j == 11)
            //            continue;
            //        else if (i == 0)
            //        {
            //            lblIoSts[j] = new Label();

            //            lblIoSts[j].Text = IoStsName[j];
            //            lblIoSts[j].TextAlign = ContentAlignment.MiddleCenter;
            //            lblIoSts[j].Location = new System.Drawing.Point(x, y);
            //            lblIoSts[j].Size = new System.Drawing.Size(grpIoSts.Width / 14, 20);

            //            x += grpIoSts.Width / 15 + grpIoSts.Width / 160;
            //            grpIoSts.Controls.Add(lblIoSts[j]);
            //        }
            //        else
            //        {
            //            lb_sts[j] = new Label();
            //            lb_sts[j].BackColor = Color.Black;
            //            lb_sts[j].Location = new System.Drawing.Point(x, y);
            //            lb_sts[j].Size = new System.Drawing.Size(grpIoSts.Width / 15, grpIoSts.Width / 15);
            //            x += grpIoSts.Width / 15 + grpIoSts.Width / 160;
            //            grpIoSts.Controls.Add(lb_sts[j]);
            //        }
            //    }
            //}//end of for i
            cBox_cardno.SelectedIndex = comboBox2.SelectedIndex = 0;
            cBox_baudrate.SelectedIndex = 2;
            timer1.Interval = 100;
        }

        private void btn_Initial_Click(object sender, EventArgs e)
        {
            ret = 0;
            ret2 = 0;
            ret3 = 0;
            ret4 = 0;
            ret5 = 0;

            ushort existcards = 0;


            ret += Master.PCI_L122_DSF._l122_dsf_open(ref existcards);
            ret2 += Master.PCI_L122_DSF._l122_dsf_open(ref existcards);
            ret3 += Master.PCI_L122_DSF._l122_dsf_open(ref existcards);
            ret4 += Master.PCI_L122_DSF._l122_dsf_open(ref existcards);
            //ret5 += Master.PCI_L122_DSF._l122_dsf_open(ref existcards);
            if (existcards == 0)
            {
                MessageBox.Show("No any PCI_L122_DSF!!!");
                return;
            }
            else
                ret = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            ret2 = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            ret3 = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            ret4 = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            ret5 = Master.PCI_L122_DSF._l122_dsf_get_switch_card_num(0, ref m_CardNo);
            if (ret == 0)
            {
                btn_Initial.BackColor = Color.Lime;
                btn_CheckSlave.Enabled = true;
                btn_Initial.Enabled = false;
            }
            else if (ret2 == 0)
            {
                btn_Initial.BackColor = Color.Lime;
                btn_CheckSlave.Enabled = true;
                btn_Initial.Enabled = false;
            }
            else if (ret3 == 0)
            {
                btn_Initial.BackColor = Color.Lime;
                btn_CheckSlave.Enabled = true;
                btn_Initial.Enabled = false;
            }
            else if (ret4 == 0)
            {
                btn_Initial.BackColor = Color.Lime;
                btn_CheckSlave.Enabled = true;
                btn_Initial.Enabled = false;
            }
            else if (ret5 == 0)
            {
                btn_Initial.BackColor = Color.Lime;
                btn_CheckSlave.Enabled = true;
                btn_Initial.Enabled = false;
            }
            else
                btn_Initial.BackColor = Color.Red;
        }

        //check slave
        private void btn_CheckSlave_Click(object sender, EventArgs e)
        {
            if (Master.PCI_L122_DSF._l122_dsf_set_ring_config(m_CardNo, m_RingNo, m_Baudrate0) != 0)
            {
                MessageBox.Show("Set ring config fail !!");
                return;
            }

            if (MNet.Basic._mnet_reset_ring(m_RingNo) != 0)
            {
                MessageBox.Show("Reset Ring fail!!");
                return;
            }

            ret = MNet.Basic._mnet_start_ring(m_RingNo);
            ret = MNet.Basic._mnet_get_ring_active_table(m_RingNo, lDevTable);

            ret2 = MNet.Basic._mnet_start_ring(m_RingNo);
            ret2 = MNet.Basic._mnet_get_ring_active_table(m_RingNo, lDevTable);

            ret3 = MNet.Basic._mnet_start_ring(m_RingNo);
            ret3 = MNet.Basic._mnet_get_ring_active_table(m_RingNo, lDevTable);

            ret4 = MNet.Basic._mnet_start_ring(m_RingNo);
            ret4 = MNet.Basic._mnet_get_ring_active_table(m_RingNo, lDevTable);

            ret5 = MNet.Basic._mnet_start_ring(m_RingNo);
            ret5 = MNet.Basic._mnet_get_ring_active_table(m_RingNo, lDevTable);
            if (ret == -74)
            {
                MessageBox.Show("No Device!");
                return;
            }
            if (ret2 == -74)
            {
                MessageBox.Show("No Device!");
                return;
            }
            if (ret3 == -74)
            {
                MessageBox.Show("No Device!");
                return;
            }
            if (ret4 == -74)
            {
                MessageBox.Show("No Device!");
                return;
            }
            if (ret5 == -74)
            {
                MessageBox.Show("No Device!");
                return;
            }
            MNet.SlaveType slavetype = 0;
            int count_m1a = 0;

            for (ushort ip = 0; ip < 64; ip++)
            {
                if ((lDevTable[ip / 32] & (0x01 << (ip % 32))) != 0)
                {
                    ret += MNet.Basic._mnet_get_slave_type(m_RingNo, ip, ref slavetype);
                    ret2 += MNet.Basic._mnet_get_slave_type(m_RingNo, ip, ref slavetype);
                    ret3 += MNet.Basic._mnet_get_slave_type(m_RingNo, ip, ref slavetype);
                    ret4 += MNet.Basic._mnet_get_slave_type(m_RingNo, ip, ref slavetype);
                    ret5 += MNet.Basic._mnet_get_slave_type(m_RingNo, ip, ref slavetype);
                    if (slavetype == MNet.SlaveType.AXIS_M1x1a)//axis module
                    {
                        if (((ret = MNet.M1A._mnet_m1a_initial(m_RingNo, ip)) == 0) || ((ret2 = MNet.M1A._mnet_m1a_initial(m_RingNo, ip)) == 0) || ((ret3 = MNet.M1A._mnet_m1a_initial(m_RingNo, ip)) == 0 || ((ret4 = MNet.M1A._mnet_m1a_initial(m_RingNo, ip)) == 0 || ((ret5 = MNet.M1A._mnet_m1a_initial(m_RingNo, ip)) == 0))))
                        {
                            count_m1a++;
                            if (count_m1a == 1 )
                            {
                                SlaveIP[0] = ip;
                                groupBox4.Text += string.Format(" IP={0}", ip);
                               
                                // initial settings from EEPROM or by API//
                                if (comboBox2.SelectedIndex == 0 )
                                    ret = MNet.M1A._mnet_m1a_recovery_from_EEPROM(m_RingNo, ip);

                                else
                                {
                                    // Recovery settings form file(*.ini) saved by MyLink//
                                    //byte[] bArray = Encoding.UTF8.GetBytes(path);
                                    //ret = MNet.M1A._mnet_m1a_load_motion_file(m_RingNo, ip, bArray);
                                }
                                if (ret != 0)
                                    MessageBox.Show("Initial fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //////////////////////

                                for (int i = 1; i < 6; i++)
                                { cmb_Mode[i].SelectedIndex = 0; }

                                btn_CheckSlave.BackColor = Color.Lime;

                                groupBox1.Enabled = grpIoSts.Enabled = true;
                               
                                btn_CheckSlave.Enabled = false;
                                timer1.Enabled = true;
                            }
                            else if (count_m1a == 2 )
                            {
                                SlaveIP1[0] = ip;
                                
                                groupBox2.Text += string.Format(" IP>{0}", ip);
                                // initial settings from EEPROM or by API//
                                if (comboBox2.SelectedIndex == 0 )
                                    ret2 = MNet.M1A._mnet_m1a_recovery_from_EEPROM(m_RingNo, ip);
                                else
                                {
                                    // Recovery settings form file(*.ini) saved by MyLink//
                                    //byte[] bArray = Encoding.UTF8.GetBytes(path);
                                    //ret = MNet.M1A._mnet_m1a_load_motion_file(m_RingNo, ip, bArray);
                                }
                                if (ret2 != 0)
                                    MessageBox.Show("Initial fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //////////////////////

                                for (int i = 1; i < 6; i++)
                                { cmb_Mode[i].SelectedIndex = 0; }

                                btn_CheckSlave.BackColor = Color.Lime;

                                
                                groupBox5.Enabled = grpIoSts.Enabled = true;
                                btn_CheckSlave.Enabled = false;
                                timer1.Enabled = true;
                            }


                            else if (count_m1a == 3)
                            {
                                SlaveIP2[0] = ip;
                                
                                groupBox7.Text += string.Format(" IP>{0}", ip);
                                // initial settings from EEPROM or by API//
                                if (comboBox2.SelectedIndex == 0)
                                    ret3 = MNet.M1A._mnet_m1a_recovery_from_EEPROM(m_RingNo, ip);
                                else
                                {
                                    // Recovery settings form file(*.ini) saved by MyLink//
                                    //byte[] bArray = Encoding.UTF8.GetBytes(path);
                                    //ret = MNet.M1A._mnet_m1a_load_motion_file(m_RingNo, ip, bArray);
                                }
                                if (ret3 != 0)
                                    MessageBox.Show("Initial fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //////////////////////

                                for (int i = 1; i < 6; i++)
                                { cmb_Mode[i].SelectedIndex = 0; }

                                btn_CheckSlave.BackColor = Color.Lime;

                                
                                groupBox6.Enabled = grpIoSts.Enabled = true;
                                btn_CheckSlave.Enabled = false;
                                timer1.Enabled = true;
                            }
                            else if (count_m1a == 4)
                            {
                                SlaveIP3[0] = ip;
                                
                                groupBox9.Text += string.Format(" IP>{0}", ip);
                                // initial settings from EEPROM or by API//
                                if (comboBox2.SelectedIndex == 0)
                                    ret4 = MNet.M1A._mnet_m1a_recovery_from_EEPROM(m_RingNo, ip);
                                else
                                {
                                    // Recovery settings form file(*.ini) saved by MyLink//
                                    //byte[] bArray = Encoding.UTF8.GetBytes(path);
                                    //ret = MNet.M1A._mnet_m1a_load_motion_file(m_RingNo, ip, bArray);
                                }
                                if (ret4 != 0)
                                    MessageBox.Show("Initial fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //////////////////////

                                for (int i = 1; i < 6; i++)
                                { cmb_Mode[i].SelectedIndex = 0; }

                                btn_CheckSlave.BackColor = Color.Lime;

                                
                                groupBox8.Enabled = grpIoSts.Enabled = true;
                                btn_CheckSlave.Enabled = false;
                                timer1.Enabled = true;
                            }

                            else if (count_m1a == 5)
                            {
                                SlaveIP4[0] = ip;
                                
                                groupBox12.Text += string.Format(" IP>{0}", ip);
                                // initial settings from EEPROM or by API//
                                if (comboBox2.SelectedIndex == 0)
                                    ret5 = MNet.M1A._mnet_m1a_recovery_from_EEPROM(m_RingNo, ip);
                                else
                                {
                                    // Recovery settings form file(*.ini) saved by MyLink//
                                    //byte[] bArray = Encoding.UTF8.GetBytes(path);
                                    //ret = MNet.M1A._mnet_m1a_load_motion_file(m_RingNo, ip, bArray);
                                }
                                if (ret5 != 0)
                                    MessageBox.Show("Initial fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //////////////////////

                                for (int i = 1; i < 6; i++)
                                { cmb_Mode[i].SelectedIndex = 0; }

                                btn_CheckSlave.BackColor = Color.Lime;

                                
                                groupBox13.Enabled = grpIoSts.Enabled = true;
                                btn_CheckSlave.Enabled = false;
                                timer1.Enabled = true;
                            }




                        }
                        else
                            btn_CheckSlave.BackColor = Color.Red;
                    }
                }
            }

            if (btn_CheckSlave.BackColor != Color.Lime)
            {
                btn_CheckSlave.BackColor = Color.Red;
                MessageBox.Show("Cann't find M1x1a!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cBox_baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_Baudrate0 = (byte)cBox_baudrate.SelectedIndex;
        }

        private void GetAllStatus()
        {
            int Cmd = 0, Pos = 0, Error = 0;
            uint Speed = 0, IoSts = 0;
            short McSts = 0;
            ushort Most = 0;


            if ((ret = MNet.M1A._mnet_m1a_get_command(m_RingNo, SlaveIP[0], ref Cmd)) == 0)
                m_Label[3].Text = Cmd.ToString();
            if ((ret2 = MNet.M1A._mnet_m1a_get_command(m_RingNo, SlaveIP1[0], ref Cmd)) == 0)
                m_Label[3].Text = Cmd.ToString();
            if ((ret3 = MNet.M1A._mnet_m1a_get_command(m_RingNo, SlaveIP2[0], ref Cmd)) == 0)
                m_Label[3].Text = Cmd.ToString();
            if ((ret4 = MNet.M1A._mnet_m1a_get_command(m_RingNo, SlaveIP3[0], ref Cmd)) == 0)
                m_Label[3].Text = Cmd.ToString();
            if ((ret5 = MNet.M1A._mnet_m1a_get_command(m_RingNo, SlaveIP4[0], ref Cmd)) == 0)
                m_Label[3].Text = Cmd.ToString();

            if ((ret = MNet.M1A._mnet_m1a_get_position(m_RingNo, SlaveIP[0], ref Pos)) == 0)
                m_Label[4].Text = Pos.ToString();
            if ((ret2 = MNet.M1A._mnet_m1a_get_position(m_RingNo, SlaveIP1[0], ref Pos)) == 0)
                m_Label[4].Text = Pos.ToString();
            if ((ret3 = MNet.M1A._mnet_m1a_get_position(m_RingNo, SlaveIP2[0], ref Pos)) == 0)
                m_Label[4].Text = Pos.ToString();
            if ((ret4 = MNet.M1A._mnet_m1a_get_position(m_RingNo, SlaveIP3[0], ref Pos)) == 0)
                m_Label[4].Text = Pos.ToString();
            if ((ret5 = MNet.M1A._mnet_m1a_get_position(m_RingNo, SlaveIP4[0], ref Pos)) == 0)
                m_Label[4].Text = Pos.ToString();

            if ((ret = MNet.M1A._mnet_m1a_get_current_speed(m_RingNo, SlaveIP[0], ref Speed)) == 0)
                m_Label[2].Text = Speed.ToString();
            if ((ret2 = MNet.M1A._mnet_m1a_get_current_speed(m_RingNo, SlaveIP1[0], ref Speed)) == 0)
                m_Label[2].Text = Speed.ToString();
            if ((ret3 = MNet.M1A._mnet_m1a_get_current_speed(m_RingNo, SlaveIP2[0], ref Speed)) == 0)
                m_Label[2].Text = Speed.ToString();
            if ((ret4 = MNet.M1A._mnet_m1a_get_current_speed(m_RingNo, SlaveIP3[0], ref Speed)) == 0)
                m_Label[2].Text = Speed.ToString();
            if ((ret5 = MNet.M1A._mnet_m1a_get_current_speed(m_RingNo, SlaveIP4[0], ref Speed)) == 0)
                m_Label[2].Text = Speed.ToString();


            if ((ret = MNet.M1A._mnet_m1a_get_io_status(m_RingNo, SlaveIP[0], ref IoSts)) == 0)
            {
                //m_Label[0].Text = IoSts.ToString("X");
                //for (int i = 0; i < 16; i++)
                //{
                //    if (i == 7 || i == 10 || i == 11)
                //        continue;
                //    else if ((IoSts & (0x01 << i)) != 0)
                //        lb_sts[i].BackColor = Color.Red;
                //    else
                //        lb_sts[i].BackColor = Color.Lime;
                //}
            }
            //if ((ret2 = MNet.M1A._mnet_m1a_get_io_status(m_RingNo, SlaveIP1[0], ref IoSts)) == 0)
            //{
            //    //m_Label[0].Text = IoSts.ToString("X");
            //    //for (int i = 0; i < 16; i++)
            //    //{
            //    //    if (i == 7 || i == 10 || i == 11)
            //    //        continue;
            //    //    else if ((IoSts & (0x01 << i)) != 0)
            //    //        lb_sts[i].BackColor = Color.Red;
            //    //    else
            //    //        lb_sts[i].BackColor = Color.Lime;
            //    //}
            //}

            //if ((ret3 = MNet.M1A._mnet_m1a_get_io_status(m_RingNo, SlaveIP2[0], ref IoSts)) == 0)
            //{
            //    //m_Label[0].Text = IoSts.ToString("X");
            //    //for (int i = 0; i < 16; i++)
            //    //{
            //    //    if (i == 7 || i == 10 || i == 11)
            //    //        continue;
            //    //    else if ((IoSts & (0x01 << i)) != 0)
            //    //        lb_sts[i].BackColor = Color.Red;
            //    //    else
            //    //        lb_sts[i].BackColor = Color.Lime;
            //    //}
            //}

            if ((ret = MNet.M1A._mnet_m1a_motion_done(m_RingNo, SlaveIP[0], ref Most)) == 0)
                m_Label[1].Text = McSts.ToString("X");
            if ((ret2 = MNet.M1A._mnet_m1a_motion_done(m_RingNo, SlaveIP1[0], ref Most)) == 0)
                m_Label[1].Text = McSts.ToString("X");
            if ((ret3 = MNet.M1A._mnet_m1a_motion_done(m_RingNo, SlaveIP2[0], ref Most)) == 0)
                m_Label[1].Text = McSts.ToString("X");
            if ((ret4 = MNet.M1A._mnet_m1a_motion_done(m_RingNo, SlaveIP3[0], ref Most)) == 0)
                m_Label[1].Text = McSts.ToString("X");
            if ((ret5 = MNet.M1A._mnet_m1a_motion_done(m_RingNo, SlaveIP4[0], ref Most)) == 0)
                m_Label[1].Text = McSts.ToString("X");



            if ((ret = MNet.M1A._mnet_m1a_get_error_counter(m_RingNo, SlaveIP[0], ref Error)) == 0)
                m_Label[5].Text = Error.ToString();
            if ((ret2 = MNet.M1A._mnet_m1a_get_error_counter(m_RingNo, SlaveIP1[0], ref Error)) == 0)
                m_Label[5].Text = Error.ToString();
            if ((ret3 = MNet.M1A._mnet_m1a_get_error_counter(m_RingNo, SlaveIP2[0], ref Error)) == 0)
                m_Label[5].Text = Error.ToString();
            if ((ret4 = MNet.M1A._mnet_m1a_get_error_counter(m_RingNo, SlaveIP3[0], ref Error)) == 0)
                m_Label[5].Text = Error.ToString();
            if ((ret5 = MNet.M1A._mnet_m1a_get_error_counter(m_RingNo, SlaveIP4[0], ref Error)) == 0)
                m_Label[5].Text = Error.ToString();



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetAllStatus();
        }

        //move botton 1
        private void Move_Click(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text));

                        break;
                    case 1://abs
                        ret = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text));
 
                        break;
                    case 2://conti
                        ret = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP[0], 1);
 
                        break;
                    case 3://home
                        ret = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP[0], 1);
 
                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP[0]);
  
            }
            else if (no == 2)//negative
            {
                ret = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
 
                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text) * -1);

                        break;
                    case 1://abs
                        ret = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text) * -1);

                        break;
                    case 2://conti
                        ret = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP[0], 0);

                        break;
                    case 3://home
                        ret = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP[0], 0);

                        break;
                }
            }
        }

        //move botton 2
        private void Move_Click1(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret2 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP1[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                         ret2 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 1://abs
                          ret2 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 2://conti
                        ret2 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP1[0], 1);
                        break;
                    case 3://home
                        ret2 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP1[0], 1);
                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret2 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP1[0]);
            }
            else if (no == 2)//negative
            {
                ret2 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP1[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                          ret2 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 1://abs
                        ret2 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 2://conti
                        ret2 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP1[0], 0);
                        break;
                    case 3://home
                        ret2 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP1[0], 0);
                        break;
                }
            }
        }

        //move botton 3
        private void Move_Click2(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret3 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP2[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret3 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 1://abs
                        ret3 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 2://conti
                        ret3 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP2[0], 1);
                        break;
                    case 3://home
                        ret3 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP2[0], 1);
                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret3 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP2[0]);
            }
            else if (no == 2)//negative
            {
                ret3 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP2[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret3 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 1://abs
                        ret3 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 2://conti
                        ret3 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP2[0], 0);
                        break;
                    case 3://home
                        ret3 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP2[0], 0);
                        break;
                }
            }
        }

        //move botton 4
        private void Move_Click3(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret4 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP3[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret4 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 1://abs
                        ret4 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 2://conti
                        ret4 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP3[0], 1);
                        break;
                    case 3://home
                        ret4 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP3[0], 1);
                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret4 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP3[0]);
            }
            else if (no == 2)//negative
            {
                ret4 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP3[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret4 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 1://abs
                        ret4 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 2://conti
                        ret4 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP3[0], 0);
                        break;
                    case 3://home
                        ret4 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP3[0], 0);
                        break;
                }
            }
        }
        //move botton 5
        private void Move_Click4(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret5 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP4[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret5 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 1://abs
                        ret5 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 2://conti
                        ret5 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP4[0], 1);
                        break;
                    case 3://home
                        ret5 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP4[0], 1);
                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret5 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP4[0]);
            }
            else if (no == 2)//negative
            {
                ret5 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP4[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret5 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 1://abs
                        ret5 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 2://conti
                        ret5 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP4[0], 0);
                        break;
                    case 3://home
                        ret5 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP4[0], 0);
                        break;
                }
            }
        }


        //move botton all
        private void Move_Click5(object sender, EventArgs e)
        {


            int no = (int)(sender as Button).Tag;

            if (no == 0)//positive
            {
                ret = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret2 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP1[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret3 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP2[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret4 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP3[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret5 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP4[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                

                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text));
                        ret2 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text));
                        ret3 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text));
                        ret4 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text));
                        ret5 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 1://abs
                        ret = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text));
                        ret2 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text));
                        ret3 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text));
                        ret4 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text));
                        ret5 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text));
                        break;
                    case 2://conti
                        ret = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP[0], 1);
                        ret2 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP1[0], 1);
                        ret3 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP2[0], 1);
                        ret4 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP3[0], 1);
                        ret5 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP4[0], 1);
                        break;
                    case 3://home
                        ret = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP[0], 1);
                        ret2 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP1[0], 1);
                        ret3 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP2[0], 1);
                        ret4 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP3[0], 1);
                        ret5 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP4[0], 1);

                        break;
                }
            }
            else if (no == 1)//stop
            {
                ret = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP[0]);
                ret2 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP1[0]);
                ret3 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP2[0]);
                ret4 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP3[0]);
                ret5 = MNet.M1A._mnet_m1a_emg_stop(m_RingNo, SlaveIP4[0]);

            }
            else if (no == 2)//negative
            {
                ret = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret2 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP1[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret3 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP2[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret4 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP3[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));
                ret5 = MNet.M1A._mnet_m1a_set_tmove_speed(m_RingNo, SlaveIP4[0], uint.Parse(tb_Set[1].Text), uint.Parse(tb_Set[2].Text), float.Parse(tb_Set[3].Text), float.Parse(tb_Set[4].Text));

                
                switch (cmb_Mode[0].SelectedIndex)
                {
                    case 0://relative
                        ret = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text) * -1);
                        ret2 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text) * -1);
                        ret3 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text) * -1);
                        ret4 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text) * -1);
                        ret5 = MNet.M1A._mnet_m1a_start_r_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text) * -1);
                        break;
                    case 1://abs
                        ret = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP[0], int.Parse(tb_Set[0].Text) * -1);
                        ret2 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP1[0], int.Parse(tb_Set[0].Text) * -1);
                        ret3 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP2[0], int.Parse(tb_Set[0].Text) * -1);
                        ret4 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP3[0], int.Parse(tb_Set[0].Text) * -1);
                        ret5 = MNet.M1A._mnet_m1a_start_a_move(m_RingNo, SlaveIP4[0], int.Parse(tb_Set[0].Text) * -1);

                        break;
                    case 2://conti
                        ret = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP[0], 0);
                        ret2 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP1[0], 0);
                        ret3 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP2[0], 0);
                        ret4 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP3[0], 0);
                        ret5 = MNet.M1A._mnet_m1a_v_move(m_RingNo, SlaveIP4[0], 0);
                        break;
                    case 3://home
                        ret = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP[0], 0);
                        ret2 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP1[0], 0);
                        ret3 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP2[0], 0);
                        ret4 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP3[0], 0);
                        ret5 = MNet.M1A._mnet_m1a_start_home_move(m_RingNo, SlaveIP4[0], 0);
                        break;
                }
            }
        }

        

        private void Reset_Click(object sender, EventArgs e)
        {
            int no = (int)(sender as Button).Tag;

            m_Label[3 + no * 6].Text = m_Label[4 + no * 6].Text = m_Label[5 + no * 6].Text = "0";
            MNet.M1A._mnet_m1a_set_command(m_RingNo, SlaveIP[no], 0);
            MNet.M1A._mnet_m1a_set_position(m_RingNo, SlaveIP[no], 0);
            MNet.M1A._mnet_m1a_reset_error_counter(m_RingNo, SlaveIP[no]);

            MNet.M1A._mnet_m1a_set_command(m_RingNo, SlaveIP1[no], 0);
            MNet.M1A._mnet_m1a_set_position(m_RingNo, SlaveIP1[no], 0);
            MNet.M1A._mnet_m1a_reset_error_counter(m_RingNo, SlaveIP1[no]);

            MNet.M1A._mnet_m1a_set_command(m_RingNo, SlaveIP2[no], 0);
            MNet.M1A._mnet_m1a_set_position(m_RingNo, SlaveIP2[no], 0);
            MNet.M1A._mnet_m1a_reset_error_counter(m_RingNo, SlaveIP2[no]);

            MNet.M1A._mnet_m1a_set_command(m_RingNo, SlaveIP3[no], 0);
            MNet.M1A._mnet_m1a_set_position(m_RingNo, SlaveIP3[no], 0);
            MNet.M1A._mnet_m1a_reset_error_counter(m_RingNo, SlaveIP3[no]);

            MNet.M1A._mnet_m1a_set_command(m_RingNo, SlaveIP4[no], 0);
            MNet.M1A._mnet_m1a_set_position(m_RingNo, SlaveIP4[no], 0);
            MNet.M1A._mnet_m1a_reset_error_counter(m_RingNo, SlaveIP4[no]);
        }
        //servo on 
        private void ServerOn_Checked(object sender, EventArgs e)
        {
            int no = (int)(sender as CheckBox).Tag;
            bool on_off = false;

            if (cb_SvOn[no].Checked )
            {
                on_off = true;
            }

            else
                on_off = false;

            if (MNet.M1A._mnet_m1a_set_svon(m_RingNo, SlaveIP[no], (ushort)(on_off ? 1 : 0)) != 0)
                MessageBox.Show(string.Format("Set ServerON fail at axis:{0}", SlaveIP[no]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MNet.M1A._mnet_m1a_set_svon(m_RingNo, SlaveIP1[no], (ushort)(on_off ? 1 : 0)) != 0)
                MessageBox.Show(string.Format("Set ServerON fail at axis:{0}", SlaveIP1[no]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MNet.M1A._mnet_m1a_set_svon(m_RingNo, SlaveIP2[no], (ushort)(on_off ? 1 : 0)) != 0)
                MessageBox.Show(string.Format("Set ServerON fail at axis:{0}", SlaveIP2[no]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MNet.M1A._mnet_m1a_set_svon(m_RingNo, SlaveIP3[no], (ushort)(on_off ? 1 : 0)) != 0)
                MessageBox.Show(string.Format("Set ServerON fail at axis:{0}", SlaveIP3[no]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MNet.M1A._mnet_m1a_set_svon(m_RingNo, SlaveIP4[no], (ushort)(on_off ? 1 : 0)) != 0)
                MessageBox.Show(string.Format("Set ServerON fail at axis:{0}", SlaveIP4[no]), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void ComboBox_Selected_Changed(object sender, EventArgs e)
        {
            int index = (int)(sender as ComboBox).Tag;

            if (index != 0)//axis0
            {
                //set home config before home move
                ret = MNet.M1A._mnet_m1a_set_home_config(m_RingNo, SlaveIP[0], (ushort)cmb_Mode[1].SelectedIndex, (ushort)cmb_Mode[2].SelectedIndex, (ushort)cmb_Mode[3].SelectedIndex, (ushort)cmb_Mode[4].SelectedIndex, (ushort)cmb_Mode[5].SelectedIndex);

            }
            if (index != 0)//axis1
            {
                //set home config before home move
                ret2 = MNet.M1A._mnet_m1a_set_home_config(m_RingNo, SlaveIP1[0], (ushort)cmb_Mode[1].SelectedIndex, (ushort)cmb_Mode[2].SelectedIndex, (ushort)cmb_Mode[3].SelectedIndex, (ushort)cmb_Mode[4].SelectedIndex, (ushort)cmb_Mode[5].SelectedIndex);

            }
            if (index != 0)//axis2
            {
                //set home config before home move
                ret3 = MNet.M1A._mnet_m1a_set_home_config(m_RingNo, SlaveIP2[0], (ushort)cmb_Mode[1].SelectedIndex, (ushort)cmb_Mode[2].SelectedIndex, (ushort)cmb_Mode[3].SelectedIndex, (ushort)cmb_Mode[4].SelectedIndex, (ushort)cmb_Mode[5].SelectedIndex);

            }
            if (index != 0)//axis3
            {
                //set home config before home move
                ret4 = MNet.M1A._mnet_m1a_set_home_config(m_RingNo, SlaveIP3[0], (ushort)cmb_Mode[1].SelectedIndex, (ushort)cmb_Mode[2].SelectedIndex, (ushort)cmb_Mode[3].SelectedIndex, (ushort)cmb_Mode[4].SelectedIndex, (ushort)cmb_Mode[5].SelectedIndex);

            }
            if (index != 0)//axis4
            {
                //set home config before home move
                ret5 = MNet.M1A._mnet_m1a_set_home_config(m_RingNo, SlaveIP4[0], (ushort)cmb_Mode[1].SelectedIndex, (ushort)cmb_Mode[2].SelectedIndex, (ushort)cmb_Mode[3].SelectedIndex, (ushort)cmb_Mode[4].SelectedIndex, (ushort)cmb_Mode[5].SelectedIndex);

            }



            if (ret != 0)
                MessageBox.Show("Set Home configuration fail!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        private void btn_exit_Click(object sender, EventArgs e)
        {
            ret = MNet.Basic._mnet_close();
            ret2 = MNet.Basic._mnet_close();
            ret3 = MNet.Basic._mnet_close();
            ret4 = MNet.Basic._mnet_close();
            ret5 = MNet.Basic._mnet_close();

            ret = Master.PCI_L122_DSF._l122_dsf_close(m_CardNo);
            ret2 = Master.PCI_L122_DSF._l122_dsf_close(m_CardNo);
            ret3 = Master.PCI_L122_DSF._l122_dsf_close(m_CardNo);
            ret4 = Master.PCI_L122_DSF._l122_dsf_close(m_CardNo);
            ret5 = Master.PCI_L122_DSF._l122_dsf_close(m_CardNo);
            this.Close();
        }

        private void btn_Getfile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
            openFileDialog1.Filter = "initial file(*.ini)|*.ini";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;///get the initial file(PCI_M114GH.ini) saved by MyLink
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 1)
                btn_Getfile.Enabled = true;
            else
                btn_Getfile.Enabled = false;
        }
    }
}