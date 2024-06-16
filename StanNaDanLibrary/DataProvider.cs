using NHibernate.Linq;
using StanNaDanLibrary.Entiteti;

namespace StanNaDanLibrary;

public class DataProvider
{
    #region Poslovnica

    public static Result<List<PoslovnicaView>, ErrorMessage> VratiSvePoslovnice()
    {
        ISession? s = null;
        List<PoslovnicaView> poslovnice = new();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Poslovnica> svePoslovnice = from o in s.Query<Poslovnica>()
                                                    select o;

            foreach (Poslovnica p in svePoslovnice)
            {
                poslovnice.Add(new PoslovnicaView(p));
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve poslovnice.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return poslovnice;
    }

    public async static Task<Result<bool, ErrorMessage>> DodajPoslovnicuAsync(PoslovnicaView p)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Poslovnica poslovnica = new()
            {
                Adresa = p.Adresa,
                RadnoVreme = p.RadnoVreme
            };

            await s.SaveOrUpdateAsync(poslovnica);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće dodati poslovnicu.".ToError(404);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    public async static Task<Result<PoslovnicaView, ErrorMessage>> IzmeniPoslovnicuAsync(int id, PoslovnicaView p)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Poslovnica o = s.Load<Poslovnica>(id);
            o.Adresa = p.Adresa;
            o.RadnoVreme = p.RadnoVreme;
            //o.Sef = p.Sef;

            await s.UpdateAsync(o);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće ažurirati poslovnicu.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return p;
    }

    public async static Task<Result<PoslovnicaView, ErrorMessage>> VratiPoslovnicuAsync(int id)
    {
        ISession? s = null;

        PoslovnicaView poslovnicaView = default!;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Poslovnica o = await s.LoadAsync<Poslovnica>(id);
            poslovnicaView = new PoslovnicaView(o);
        }
        catch (Exception)
        {
            return "Nemoguće vratiti poslovnicu sa zadatim ID-jem.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return poslovnicaView;
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiPoslovnicuAsync(int id)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Poslovnica o = await s.LoadAsync<Poslovnica>(id);

            await s.DeleteAsync(o);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati poslovnicu.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }

    #endregion

    #region Zaposleni
    public static Result<List<ZaposleniView>, ErrorMessage> VratiSveZaposlene()
    {
        ISession? s = null;
        List<ZaposleniView> zaposleniToReturn = new List<ZaposleniView>();

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            IEnumerable<Zaposleni> sviZaposleni = s.Query<Zaposleni>();

            foreach (Zaposleni zaposleni in sviZaposleni)
            {
                if (zaposleni is Sef sef)
                {
                    zaposleniToReturn.Add(new SefView(sef));
                    Poslovnica poslovnicaToUpdate = s.Load<Poslovnica>(sef.Poslovnica.ID);
                    poslovnicaToUpdate.Sef = sef;
                    s.SaveOrUpdate(poslovnicaToUpdate);
                    s.Flush();
                }
                else if (zaposleni is Agent agent)
                {
                    zaposleniToReturn.Add(new AgentView(agent));
                }
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti sve zaposlene.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return zaposleniToReturn;
    }

    public static async Task<Result<ZaposleniView, ErrorMessage>> VratiZaposlenogAsync(string mbr)
    {
        ISession? s = null;

        ZaposleniView zaposleniView;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
            Agent agent = await s.LoadAsync<Agent>(mbr);

            if (agent == null)
            {
                Sef sef = await s.LoadAsync<Sef>(mbr);
                zaposleniView = new SefView(sef);
            }
            else if (agent != null)
            {
                zaposleniView = new AgentView(agent);

            }
            else 
            {
                return "Nepoznati tip zaposlenog.".ToError(400);
            }
        }
        catch (Exception)
        {
            return "Nemoguće vratiti zaposlenog sa zadatim MBR-om.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return zaposleniView;
    }

    public static async Task<Result<bool, ErrorMessage>> ObrisiZaposlenogAsync(string mbr)
    {
        ISession? s = null;

        try
        {
            s = DataLayer.GetSession();

            if (!(s?.IsConnected ?? false))
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }

            Zaposleni zaposleni = await s.LoadAsync<Zaposleni>(mbr);

            await s.DeleteAsync(zaposleni);
            await s.FlushAsync();
        }
        catch (Exception)
        {
            return "Nemoguće obrisati zaposlenog.".ToError(400);
        }
        finally
        {
            s?.Close();
            s?.Dispose();
        }

        return true;
    }
    #endregion

    #region Agent

    public static Result<List<AgentView>, ErrorMessage> VratiSveAgente()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.Agent> sviAgenti = from agent
                                                                         in session.Query<StanNaDanLibrary.Entiteti.Agent>()
                                                                         select agent;

                List<AgentView> agentiToReturn = sviAgenti.Select(z => new AgentView(z)).ToList();

                return agentiToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja agenata: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<AgentView, ErrorMessage> VratiAgenta(string mbr)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Agent agent = session.Get<Agent>(mbr);
                if (agent != null)
                {
                    AgentView agentView = new AgentView(agent);
                    return agentView;
                }
                else
                {
                    return $"Agent sa MBR: {mbr} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja agenta: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNovogAgenta(int idPoslovnice, AgentView agentView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Poslovnica poslovnica = session.Load<Poslovnica>(idPoslovnice);

                Agent agent = new Agent
                {
                    MBR = agentView.MBR,
                    Ime = agentView.Ime,
                    Prezime = agentView.Prezime,
                    DatumZaposlenja = agentView.DatumZaposlenja,
                    StrucnaSprema = agentView.StrucnaSprema,
                    Poslovnica = poslovnica
                };
                poslovnica.Zaposleni.Add(agent);

                session.SaveOrUpdate(agent);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja novog agenta: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<AgentView, ErrorMessage> IzmeniAgenta(string mbr, AgentView agentView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Agent agent = session.Get<Agent>(mbr);

                if (agent != null)
                {
                    agent.Ime = agentView.Ime;
                    agent.Prezime = agentView.Prezime;
                    agent.DatumZaposlenja = agentView.DatumZaposlenja;
                    agent.StrucnaSprema = agentView.StrucnaSprema;

                    session.Update(agent);
                    session.Flush();

                    return agentView;
                }
                else
                {
                    return $"Agent sa MBR: {agentView.MBR} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene agenta: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Sef
    public static Result<List<SefView>, ErrorMessage> VratiSveSefove()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.Sef> sviSefovi = from sef
                                                                       in session.Query<StanNaDanLibrary.Entiteti.Sef>()
                                                                       select sef;

                List<SefView> sefoviToReturn = sviSefovi.Select(z => new SefView(z)).ToList();

                return sefoviToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja sefova: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<SefView, ErrorMessage> VratiSefa(string mbr)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Sef sef = session.Get<Sef>(mbr);
                if (sef != null)
                {
                    SefView sefView = new SefView(sef);
                    return sefView;
                }
                else
                {
                    return $"Šef sa MBR: {mbr} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja šefa: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNovogSefa(int idPoslovnice, SefView sefView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Poslovnica poslovnica = session.Load<Poslovnica>(idPoslovnice);
                if (poslovnica.Sef != null)
                {
                    return "Poslovnica može imati samo jednog šefa! Morate ukloniti postojećeg da biste postavili novog.".ToError(400);
                }
                Sef sef = new Sef
                {
                    MBR = sefView.MBR,
                    Ime = sefView.Ime,
                    Prezime = sefView.Prezime,
                    DatumZaposlenja = sefView.DatumZaposlenja,
                    DatumPostavljanja = sefView.DatumPostavljanja,
                    Poslovnica = poslovnica
                };
                poslovnica.Sef = sef;
                poslovnica.Zaposleni.Add(sef);

                session.SaveOrUpdate(poslovnica);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja novog šefa: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniSefa(string mbr,SefView izmenjeniSef)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Sef sef = session.Get<Sef>(mbr);
                if (sef != null)
                {
                    sef.Ime = izmenjeniSef.Ime;
                    sef.Prezime = izmenjeniSef.Prezime;
                    sef.DatumZaposlenja = izmenjeniSef.DatumZaposlenja;
                    sef.DatumPostavljanja = izmenjeniSef.DatumPostavljanja;

                    session.Update(sef);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Šef sa MBR {izmenjeniSef.MBR} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene šefa: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region SpoljniSaradnik

    public static Result<List<SpoljniSaradnikView>, ErrorMessage> VratiSveSpoljneSaradnike()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.SpoljniSaradnik> sviSpoljni = from spoljniSaradnik
                                                                                    in session.Query<StanNaDanLibrary.Entiteti.SpoljniSaradnik>()
                                                                                    select spoljniSaradnik;

                List<SpoljniSaradnikView> spoljniToReturn = sviSpoljni.Select(s => new SpoljniSaradnikView(s)).ToList();

                return spoljniToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih spoljnih saradnika: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<SpoljniSaradnikView>, ErrorMessage> VratiSveSpoljneSaradnikeAgenta(string mbrAgentaAngazovanja)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.SpoljniSaradnik> sviSpoljni = from spoljniSaradnik
                                                                                    in session.Query<StanNaDanLibrary.Entiteti.SpoljniSaradnik>()
                                                                                    where spoljniSaradnik.ID.AgentAngazovanja.MBR == mbrAgentaAngazovanja
                                                                                    select spoljniSaradnik;

                List<SpoljniSaradnikView> spoljniToReturn = sviSpoljni.Select(s => new SpoljniSaradnikView(s)).ToList();

                return spoljniToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih spoljnih saradnika agenta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<SpoljniSaradnikView, ErrorMessage> VratiSpoljnogSaradnika(string mbrAgentaAngazovanja, int idSpoljnogSaradnika)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Agent agentAngazovanja = session.Load<Agent>(mbrAgentaAngazovanja);
                SpoljniSaradnikId ssID = new SpoljniSaradnikId
                {
                    AgentAngazovanja = agentAngazovanja,
                    IdSaradnika = idSpoljnogSaradnika
                };
                SpoljniSaradnik spoljniSaradnik = session.Load<SpoljniSaradnik>(ssID);
                PoslovnicaView poslovnicaView = new PoslovnicaView
                {
                    ID = spoljniSaradnik.ID.AgentAngazovanja.Poslovnica.ID,
                    Adresa = spoljniSaradnik.ID.AgentAngazovanja.Poslovnica.Adresa,
                    RadnoVreme = spoljniSaradnik.ID.AgentAngazovanja.Poslovnica.RadnoVreme,
                    Sef = new SefView(spoljniSaradnik.ID.AgentAngazovanja.Poslovnica.Sef == null ? spoljniSaradnik.ID.AgentAngazovanja.Poslovnica.Sef : null)
                };
                AgentView agentAngazovanjaView = new AgentView
                {
                    DatumZaposlenja = spoljniSaradnik.ID.AgentAngazovanja.DatumZaposlenja,
                    Ime = spoljniSaradnik.ID.AgentAngazovanja.Ime,
                    Prezime = spoljniSaradnik.ID.AgentAngazovanja.Prezime,
                    MBR = spoljniSaradnik.ID.AgentAngazovanja.MBR,
                    StrucnaSprema = spoljniSaradnik.ID.AgentAngazovanja.StrucnaSprema,
                    Poslovnica = poslovnicaView
                };
                SpoljniSaradnikView spoljniToReturn = new SpoljniSaradnikView
                {
                    ID = new SpoljniSaradnikIdView(ssID),
                    DatumAngazovanja = spoljniSaradnik.DatumAngazovanja,
                    ProcenatOdNajma = spoljniSaradnik.ProcenatOdNajma,
                    Ime = spoljniSaradnik.Ime,
                    Prezime = spoljniSaradnik.Prezime,
                    Telefon = spoljniSaradnik.Telefon
                };

                return spoljniToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja spoljnog saradnika: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiSpoljnogSaradnika(string mbrAgentaAngazovanja, int idSpoljnog)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                SpoljniSaradnikId ssID = new SpoljniSaradnikId
                {
                    AgentAngazovanja = session.Load<Agent>(mbrAgentaAngazovanja),
                    IdSaradnika = idSpoljnog
                };
                SpoljniSaradnik spoljniSaradnik = session.Load<SpoljniSaradnik>(ssID);
                string ime = spoljniSaradnik.Ime;
                session.Delete(spoljniSaradnik);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja spoljnog saradnika: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNovogSpoljnogSaradnika(SpoljniSaradnikView ssView, string mbrAgentaAngazovanja)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Agent agent = session.Load<Agent>(mbrAgentaAngazovanja);
                SpoljniSaradnikId ssID = new SpoljniSaradnikId
                {
                    AgentAngazovanja = agent,
                    IdSaradnika = ssView.ID.IdSaradnika
                };
                SpoljniSaradnik spoljniSaradnik = new SpoljniSaradnik
                {
                    Ime = ssView.Ime,
                    Prezime = ssView.Prezime,
                    DatumAngazovanja = ssView.DatumAngazovanja,
                    ProcenatOdNajma = ssView.ProcenatOdNajma,
                    Telefon = ssView.Telefon,
                    ID = ssID
                };
                agent.AngazovaniSaradnici.Add(spoljniSaradnik);

                session.SaveOrUpdate(spoljniSaradnik);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja novog spoljnog saradnika: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Kvart

    public static Result<List<KvartView>, ErrorMessage> VratiSveKvartove()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.Kvart> sviKvartovi = from kvart
                                                                           in session.Query<StanNaDanLibrary.Entiteti.Kvart>()
                                                                           select kvart;

                List<KvartView> kvartovi = sviKvartovi.Select(k => new KvartView(k)).ToList();

                return kvartovi;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih kvartova: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<KvartView>, ErrorMessage> VratiSveKvartovePoslovnice(int idPoslovnice)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<StanNaDanLibrary.Entiteti.Kvart> sviKvartovi = from kvart
                                                                           in session.Query<StanNaDanLibrary.Entiteti.Kvart>()
                                                                           where kvart.PoslovnicaZaduzenaZaNjega.ID == idPoslovnice
                                                                           select kvart;

                List<KvartView> kvartovi = sviKvartovi.Select(k => new KvartView(k)).ToList();

                return kvartovi;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih kvartova poslovnice: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<KvartView, ErrorMessage> VratiKvart(int idKvarta)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Kvart kvart = session.Load<Kvart>(idKvarta);
                KvartView kvartView = new KvartView(kvart);
                return kvartView;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja kvarta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNoviKvart(int idPoslovnice, KvartView kvartView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Poslovnica poslovnica = session.Load<Poslovnica>(idPoslovnice);

                Kvart kvart = new Kvart
                {
                    GradskaZona = kvartView.GradskaZona,
                    PoslovnicaZaduzenaZaNjega = poslovnica
                };
                poslovnica.Kvartovi.Add(kvart);

                session.SaveOrUpdate(kvart);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja novog kvarta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniKvart(int idKvarta, KvartView noviPodaci)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Kvart kvart = session.Load<Kvart>(idKvarta);
                if (kvart == null)
                {
                    return "Kvart nije pronađen.".ToError(404);
                }

                kvart.GradskaZona = noviPodaci.GradskaZona;

                session.SaveOrUpdate(kvart);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene kvarta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiKvart(int idKvarta)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Kvart kvart = session.Load<Kvart>(idKvarta);
                if (kvart == null)
                {
                    return "Kvart nije pronađen.".ToError(404);
                }

                session.Delete(kvart);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja kvarta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Vlasnik
    public static Result<bool, ErrorMessage> ObrisiVlasnika(string jmbg = null, string pib = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                if (jmbg != null)
                {
                    FizickoLice fizickoLice = session.Load<FizickoLice>(jmbg);
                    if (fizickoLice == null)
                    {
                        return "Fizičko lice nije pronađeno.".ToError(404);
                    }
                    int idVlasnika = fizickoLice.Vlasnik.IdVlasnika;
                    session.Delete(fizickoLice);
                    session.Flush();
                    return true;
                }
                if (pib != null)
                {
                    PravnoLice pravnoLice = session.Load<PravnoLice>(pib);
                    if (pravnoLice == null)
                    {
                        return "Pravno lice nije pronađeno.".ToError(404);
                    }
                    int idVlasnika = pravnoLice.Vlasnik.IdVlasnika;
                    session.Delete(pravnoLice);
                    session.Flush();
                    return true;
                }
                return "Niste naveli JMBG ili PIB vlasnika.".ToError(400);
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja vlasnika: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }
    #endregion

    #region FizickoLice

    public static Result<List<FizickoLiceView>, ErrorMessage> VratiSvaFizickaLica()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<FizickoLice> svaFizickaLica = session.Query<FizickoLice>().ToList();
                List<FizickoLiceView> fizickaLicaToReturn = new List<FizickoLiceView>();

                foreach (FizickoLice fl in svaFizickaLica)
                {
                    fizickaLicaToReturn.Add(new FizickoLiceView(fl));
                }

                return fizickaLicaToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih fizičkih lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<FizickoLiceView, ErrorMessage> VratiFizickoLice(string jmbg)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Get<FizickoLice>(jmbg);
                if (fizickoLice != null)
                {
                    return new FizickoLiceView(fizickoLice);
                }
                else
                {
                    return $"Fizičko lice sa JMBG {jmbg} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja fizičkog lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNovoFizickoLice(FizickoLiceView fizickoLiceView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Vlasnik vlasnik = new()
                {
                    Banka = fizickoLiceView.Vlasnik.Banka,
                    BrojBankovnogRacuna = fizickoLiceView.Vlasnik.BrojBankovnogRacuna
                };

                FizickoLice fizickoLice = new()
                {
                    JMBG = fizickoLiceView.JMBG,
                    Ime = fizickoLiceView.Ime,
                    Prezime = fizickoLiceView.Prezime,
                    ImeRoditelja = fizickoLiceView.ImeRoditelja,
                    MestoStanovanja = fizickoLiceView.MestoStanovanja,
                    AdresaStanovanja = fizickoLiceView.MestoStanovanja,
                    DatumRodjenja = fizickoLiceView.DatumRodjenja,
                    Drzava = fizickoLiceView.Drzava,
                    Email = fizickoLiceView.Email,
                    Vlasnik = vlasnik
                };

                session.SaveOrUpdate(fizickoLice);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja novog fizičkog lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniFizickoLice(FizickoLiceView izmenjenoFizickoLice)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Get<FizickoLice>(izmenjenoFizickoLice.JMBG);
                if (fizickoLice != null)
                {
                    fizickoLice.Ime = izmenjenoFizickoLice.Ime;
                    fizickoLice.Prezime = izmenjenoFizickoLice.Prezime;
                    fizickoLice.ImeRoditelja = izmenjenoFizickoLice.ImeRoditelja;
                    fizickoLice.MestoStanovanja = izmenjenoFizickoLice.MestoStanovanja;
                    fizickoLice.AdresaStanovanja = izmenjenoFizickoLice.AdresaStanovanja;
                    fizickoLice.DatumRodjenja = izmenjenoFizickoLice.DatumRodjenja;
                    fizickoLice.Drzava = izmenjenoFizickoLice.Drzava;
                    fizickoLice.Email = izmenjenoFizickoLice.Email;

                    fizickoLice.Vlasnik.Banka = izmenjenoFizickoLice.Vlasnik.Banka;
                    fizickoLice.Vlasnik.BrojBankovnogRacuna = izmenjenoFizickoLice.Vlasnik.BrojBankovnogRacuna;

                    session.Update(fizickoLice);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Fizičko lice sa JMBG {izmenjenoFizickoLice.JMBG} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene fizičkog lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiFizickoLice(string jmbg)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Get<FizickoLice>(jmbg);
                if (fizickoLice != null)
                {
                    session.Delete(fizickoLice);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Fizičko lice sa JMBG {jmbg} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja fizičkog lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region PravnoLice

    public static Result<List<PravnoLiceView>, ErrorMessage> VratiSvaPravnaLica()
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<PravnoLice> svaPravnaLica = session.Query<PravnoLice>().ToList();
                List<PravnoLiceView> pravnaLicaToReturn = new List<PravnoLiceView>();

                foreach (PravnoLice pl in svaPravnaLica)
                {
                    pravnaLicaToReturn.Add(new PravnoLiceView(pl));
                }

                return pravnaLicaToReturn;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih pravnih lica: {ex.Message}.".ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> DodajNovoPravnoLice(PravnoLiceView pravnoLiceView)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();

            if (session != null && session.IsOpen)
            {
                Vlasnik vlasnik = new()
                {
                    Banka = pravnoLiceView.Vlasnik.Banka,
                    BrojBankovnogRacuna = pravnoLiceView.Vlasnik.BrojBankovnogRacuna
                };

                PravnoLice pravnoLice = new()
                {
                    PIB = pravnoLiceView.PIB,
                    Naziv = pravnoLiceView.Naziv,
                    AdresaSedista = pravnoLiceView.AdresaSedista,
                    ImeKontaktOsobe = pravnoLiceView.ImeKontaktOsobe,
                    EmailKontaktOsobe = pravnoLiceView.EmailKontaktOsobe,
                    Vlasnik = vlasnik
                };

                session.SaveOrUpdate(pravnoLice);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return ex.FormatExceptionMessage().ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<PravnoLiceView, ErrorMessage> VratiPravnoLice(string pib)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Get<PravnoLice>(pib);
                if (pravnoLice != null)
                {
                    return new PravnoLiceView(pravnoLice);
                }
                else
                {
                    return $"Pravno lice sa PIB {pib} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return ex.FormatExceptionMessage().ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniPravnoLice(string pib, PravnoLiceView izmenjenoPravnoLice)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Get<PravnoLice>(pib);
                if (pravnoLice != null)
                {
                    pravnoLice.Naziv = izmenjenoPravnoLice.Naziv;
                    pravnoLice.AdresaSedista = izmenjenoPravnoLice.AdresaSedista;
                    pravnoLice.ImeKontaktOsobe = izmenjenoPravnoLice.ImeKontaktOsobe;
                    pravnoLice.EmailKontaktOsobe = izmenjenoPravnoLice.EmailKontaktOsobe;
                    pravnoLice.Vlasnik.Banka = izmenjenoPravnoLice.Vlasnik.Banka;
                    pravnoLice.Vlasnik.BrojBankovnogRacuna = izmenjenoPravnoLice.Vlasnik.BrojBankovnogRacuna;

                    session.Update(pravnoLice);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Pravno lice sa PIB {izmenjenoPravnoLice.PIB} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return ex.FormatExceptionMessage().ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiPravnoLice(string pib)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Get<PravnoLice>(pib);
                if (pravnoLice != null)
                {
                    session.Delete(pravnoLice);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Pravno lice sa PIB {pib} nije pronađeno.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return ex.FormatExceptionMessage().ToError(500);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Nekretnina

    public static Result<bool, ErrorMessage> DodajNekretninu(int idKvarta, int idVlasnika, KucaView kucaView = null, StanView stanView = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Kvart kvart = session.Load<Kvart>(idKvarta);
                Vlasnik vlasnik = session.Load<Vlasnik>(idVlasnika);
                if (kucaView != null)
                {
                    Kuca kuca = new Kuca
                    {
                        Ulica = kucaView.Ulica,
                        Broj = kucaView.Broj,
                        BrojKupatila = kucaView.BrojKupatila,
                        BrojSpavacihSoba = kucaView.BrojSpavacihSoba,
                        BrojTerasa = kucaView.BrojTerasa,
                        Kvadratura = kucaView.Kvadratura,
                        Kvart = kvart,
                        Vlasnik = vlasnik,
                        PosedujeInternet = kucaView.PosedujeInternet,
                        PosedujeKuhinju = kucaView.PosedujeKuhinju,
                        PosedujeTV = kucaView.PosedujeTV,
                        Spratnos = kucaView.Spratnost,
                        PosedujeDvoriste = kucaView.PosedujeDvoriste
                    };

                    vlasnik.Nekretnine.Add(kuca);
                    kvart.Nekretnine.Add(kuca);
                    session.SaveOrUpdate(kuca);
                    session.Flush();
                    return true;
                }
                else if (stanView != null)
                {
                    Stan stan = new Stan
                    {
                        Ulica = stanView.Ulica,
                        Broj = stanView.Broj,
                        BrojKupatila = stanView.BrojKupatila,
                        BrojSpavacihSoba = stanView.BrojSpavacihSoba,
                        BrojTerasa = stanView.BrojTerasa,
                        Kvadratura = stanView.Kvadratura,
                        Kvart = kvart,
                        Vlasnik = vlasnik,
                        PosedujeInternet = stanView.PosedujeInternet,
                        PosedujeKuhinju = stanView.PosedujeKuhinju,
                        PosedujeTV = stanView.PosedujeTV,
                        Sprat = stanView.Sprat,
                        PosedujeLift = stanView.PosedujeLift
                    };

                    vlasnik.Nekretnine.Add(stan);
                    kvart.Nekretnine.Add(stan);
                    session.SaveOrUpdate(stan);
                    session.Flush();
                    return true;
                }
                else
                {
                    return "Nedostaju informacije o nekretnini.".ToError(400);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja nove nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<NekretninaView>, ErrorMessage> VratiSveNekretnine()
    {
        List<NekretninaView> nekretnine = new List<NekretninaView>();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<Nekretnina> sveNekretnine = session.Query<Nekretnina>();

                foreach (Nekretnina nekretnina in sveNekretnine)
                {
                    if (nekretnina is Stan)
                    {
                        Stan stan = (Stan)nekretnina;
                        nekretnine.Add(new StanView(stan));
                    }
                    else if (nekretnina is Kuca)
                    {
                        Kuca kuca = (Kuca)nekretnina;
                        nekretnine.Add(new KucaView(kuca));
                    }
                }

                return nekretnine;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih nekretnina: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniNekretninu(int idNekretnine, KucaView kucaView = null, StanView stanView = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina? nekretnina = session.Get<Nekretnina>(idNekretnine);
                if (nekretnina != null)
                {
                    if (kucaView != null && nekretnina is Kuca kuca)
                    {
                        kuca.Ulica = kucaView.Ulica;
                        kuca.Broj = kucaView.Broj;
                        kuca.BrojKupatila = kucaView.BrojKupatila;
                        kuca.BrojSpavacihSoba = kucaView.BrojSpavacihSoba;
                        kuca.BrojTerasa = kucaView.BrojTerasa;
                        kuca.Kvadratura = kucaView.Kvadratura;
                        kuca.PosedujeInternet = kucaView.PosedujeInternet;
                        kuca.PosedujeKuhinju = kucaView.PosedujeKuhinju;
                        kuca.PosedujeTV = kucaView.PosedujeTV;
                        kuca.Spratnos = kucaView.Spratnost;
                        kuca.PosedujeDvoriste = kucaView.PosedujeDvoriste;
                    }
                    else if (stanView != null && nekretnina is Stan stan)
                    {
                        stan.Ulica = stanView.Ulica;
                        stan.Broj = stanView.Broj;
                        stan.BrojKupatila = stanView.BrojKupatila;
                        stan.BrojSpavacihSoba = stanView.BrojSpavacihSoba;
                        stan.BrojTerasa = stanView.BrojTerasa;
                        stan.Kvadratura = stanView.Kvadratura;
                        stan.PosedujeInternet = stanView.PosedujeInternet;
                        stan.PosedujeKuhinju = stanView.PosedujeKuhinju;
                        stan.PosedujeTV = stanView.PosedujeTV;
                        stan.Sprat = stanView.Sprat;
                        stan.PosedujeLift = stanView.PosedujeLift;
                    }
                    session.SaveOrUpdate(nekretnina);
                    session.Flush();
                    return true;
                }
                else
                {
                    return "Nekretnina sa datim ID-jem ne postoji.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom ažuriranja nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniNekretninu(NekretninaView izmenjenaNekretnina, FizickoLiceView flView = null, PravnoLiceView plView = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina? nekretnina = session.Get<Nekretnina>(izmenjenaNekretnina.IdNekretnine);
                Vlasnik? vlasnik = null;
                if (nekretnina != null)
                {
                    if (flView != null)
                    {
                        vlasnik = session.Get<Vlasnik>(flView.Vlasnik.IdVlasnika);
                        if (vlasnik == null)
                        {
                            var dodavanjeFizickogLica = DodajNovoFizickoLice(flView);
                            if (!dodavanjeFizickogLica.IsSuccess)
                                return false;
                            vlasnik = session.Get<Vlasnik>(flView.Vlasnik.IdVlasnika);
                        }
                    }
                    else if (plView != null)
                    {
                        vlasnik = session.Get<Vlasnik>(plView.Vlasnik.IdVlasnika);
                        if (vlasnik == null)
                        {
                            var dodavanjePravnogLica = DodajNovoPravnoLice(plView);
                            if (!dodavanjePravnogLica.IsSuccess)
                                return false;
                            vlasnik = session.Get<Vlasnik>(plView.Vlasnik.IdVlasnika);
                        }
                    }

                    nekretnina.Ulica = izmenjenaNekretnina.Ulica;
                    nekretnina.Broj = izmenjenaNekretnina.Broj;
                    nekretnina.Kvadratura = izmenjenaNekretnina.Kvadratura;
                    nekretnina.BrojTerasa = izmenjenaNekretnina.BrojTerasa;
                    nekretnina.BrojKupatila = izmenjenaNekretnina.BrojKupatila;
                    nekretnina.BrojSpavacihSoba = izmenjenaNekretnina.BrojSpavacihSoba;
                    nekretnina.PosedujeTV = izmenjenaNekretnina.PosedujeTV;
                    nekretnina.PosedujeInternet = izmenjenaNekretnina.PosedujeInternet;
                    nekretnina.PosedujeKuhinju = izmenjenaNekretnina.PosedujeKuhinju;
                    nekretnina.Vlasnik = vlasnik;

                    session.Update(nekretnina);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Nekretnina sa ID {izmenjenaNekretnina.IdNekretnine} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniStan(StanView izmenjenStan, int idVlasnika)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Stan? stan = session.Get<Stan>(izmenjenStan.IdNekretnine);
                Vlasnik? vlasnik = session.Get<Vlasnik>(idVlasnika);
                if (stan != null)
                {
                    if (vlasnik != null)
                    {
                        stan.Ulica = izmenjenStan.Ulica;
                        stan.Broj = izmenjenStan.Broj;
                        stan.Kvadratura = izmenjenStan.Kvadratura;
                        stan.BrojTerasa = izmenjenStan.BrojTerasa;
                        stan.BrojKupatila = izmenjenStan.BrojKupatila;
                        stan.BrojSpavacihSoba = izmenjenStan.BrojSpavacihSoba;
                        stan.PosedujeTV = izmenjenStan.PosedujeTV;
                        stan.PosedujeInternet = izmenjenStan.PosedujeInternet;
                        stan.PosedujeKuhinju = izmenjenStan.PosedujeKuhinju;
                        stan.Vlasnik = vlasnik;
                        stan.Sprat = izmenjenStan.Sprat;
                        stan.PosedujeLift = izmenjenStan.PosedujeLift;
                    }

                    session.Update(stan);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Stan sa ID {izmenjenStan.IdNekretnine} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene stana: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniKucu(KucaView izmenjenaKuca, int idVlasnika)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Kuca? kuca = session.Get<Kuca>(izmenjenaKuca.IdNekretnine);
                Vlasnik? vlasnik = session.Get<Vlasnik>(idVlasnika);
                if (kuca != null)
                {
                    if (vlasnik != null)
                    {
                        kuca.Ulica = izmenjenaKuca.Ulica;
                        kuca.Broj = izmenjenaKuca.Broj;
                        kuca.Kvadratura = izmenjenaKuca.Kvadratura;
                        kuca.BrojTerasa = izmenjenaKuca.BrojTerasa;
                        kuca.BrojKupatila = izmenjenaKuca.BrojKupatila;
                        kuca.BrojSpavacihSoba = izmenjenaKuca.BrojSpavacihSoba;
                        kuca.PosedujeTV = izmenjenaKuca.PosedujeTV;
                        kuca.PosedujeInternet = izmenjenaKuca.PosedujeInternet;
                        kuca.PosedujeKuhinju = izmenjenaKuca.PosedujeKuhinju;
                        kuca.Vlasnik = vlasnik;
                        kuca.Spratnos = izmenjenaKuca.Spratnost;
                        kuca.PosedujeDvoriste = izmenjenaKuca.PosedujeDvoriste;
                    }
                    session.Update(kuca);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Kuća sa ID {izmenjenaKuca.IdNekretnine} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene kuće: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiNekretninu(int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina? nekretnina = session.Get<Nekretnina>(idNekretnine);
                if (nekretnina != null)
                {
                    session.Delete(nekretnina);
                    session.Flush();
                    return true;
                }
                else
                {
                    return "Nekretnina sa datim ID-jem ne postoji.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<NekretninaView, ErrorMessage> VratiNekretninu(int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina? nekretnina = session.Get<Nekretnina>(idNekretnine);
                if (nekretnina != null)
                {
                    if (nekretnina is Kuca kuca)
                    {
                        return new KucaView(kuca);
                    }
                    else if (nekretnina is Stan stan)
                    {
                        return new StanView(stan);
                    }
                    else
                    {
                        return "Nekretnina sa datim ID-jem nije ni kuca ni stan.".ToError(404);
                    }
                }
                else
                {
                    return "Nekretnina sa datim ID-jem ne postoji.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<NekretninaView>, ErrorMessage> VratiSveNekretnineKvarta(int idKvarta)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var nekretnine = new List<NekretninaView>();

                var sviStanovi = session.Query<Stan>().Where(s => s.Kvart.ID == idKvarta).ToList();
                foreach (var stan in sviStanovi)
                {
                    nekretnine.Add(new StanView(stan));
                }

                var sveKuce = session.Query<Kuca>().Where(k => k.Kvart.ID == idKvarta).ToList();
                foreach (var kuca in sveKuce)
                {
                    nekretnine.Add(new KucaView(kuca));
                }

                return nekretnine;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja nekretnina kvarta: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<NekretninaView>, ErrorMessage> VratiSveNekretnineVlasnika(int idVlasnika)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var nekretnine = new List<NekretninaView>();

                var sviStanovi = session.Query<Stan>().Where(s => s.Vlasnik.IdVlasnika == idVlasnika).ToList();
                foreach (var stan in sviStanovi)
                {
                    nekretnine.Add(new StanView(stan));
                }

                var sveKuce = session.Query<Kuca>().Where(k => k.Vlasnik.IdVlasnika == idVlasnika).ToList();
                foreach (var kuca in sveKuce)
                {
                    nekretnine.Add(new KucaView(kuca));
                }

                return nekretnine;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja nekretnina vlasnika: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }
    #endregion

    #region DodatnaOprema

    public static Result<bool, ErrorMessage> DodajDodatnuOpremu(DodatnaOpremaView novaOprema, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                DodatnaOpremaId doID = new DodatnaOpremaId
                {
                    IdOpreme = novaOprema.ID.IdOpreme,
                    Nekretnina = nekretnina
                };

                DodatnaOprema dodatnaOprema = new DodatnaOprema
                {
                    ID = doID,
                    TipOpreme = novaOprema.TipOpreme,
                    BesplatnoKoriscenje = novaOprema.BesplatnoKoriscenje,
                    CenaKoriscenja = novaOprema.CenaKoriscenja
                };
                nekretnina.DodatnaOprema.Add(dodatnaOprema);

                session.Save(dodatnaOprema);
                session.Flush();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja nove dodatne opreme: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<DodatnaOpremaView>, ErrorMessage> VratiSvuDodatnuOpremuNekretnine(int idNekretnine)
    {
        List<DodatnaOpremaView> dodatnaOprema = new List<DodatnaOpremaView>();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<DodatnaOprema> svaOprema = from oprema
                                                       in session.Query<DodatnaOprema>()
                                                       where oprema.ID.Nekretnina.IdNekretnine == idNekretnine
                                                       select oprema;

                foreach (DodatnaOprema op in svaOprema)
                {
                    dodatnaOprema.Add(new DodatnaOpremaView
                    {
                        ID = new DodatnaOpremaIdView
                        { 
                            IdOpreme = op.ID.IdOpreme, 
                            Nekretnina = new NekretninaView(op.ID.Nekretnina) 
                        },
                        TipOpreme = op.TipOpreme,
                        BesplatnoKoriscenje = op.BesplatnoKoriscenje,
                        CenaKoriscenja = op.CenaKoriscenja
                    });
                }

                return dodatnaOprema;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih dodatnih oprema nekretnine: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<DodatnaOpremaView?, ErrorMessage> VratiDodatnuOpremu(DodatnaOpremaId id)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                DodatnaOprema? oprema = session.Get<DodatnaOprema>(id);
                if (oprema != null)
                {
                    return new DodatnaOpremaView
                    {
                        ID = new DodatnaOpremaIdView
                        {
                            IdOpreme = oprema.ID.IdOpreme,
                            Nekretnina = new NekretninaView(oprema.ID.Nekretnina)
                        },
                        TipOpreme = oprema.TipOpreme,
                        BesplatnoKoriscenje = oprema.BesplatnoKoriscenje,
                        CenaKoriscenja = oprema.CenaKoriscenja
                    };
                }
                else
                {
                    return $"Dodatna oprema sa ID {id} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja dodatne opreme: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> IzmeniDodatnuOpremu(DodatnaOpremaView izmenjenaOprema, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                DodatnaOpremaId doID = new DodatnaOpremaId
                {
                    IdOpreme = izmenjenaOprema.ID.IdOpreme,
                    Nekretnina = session.Get<Nekretnina>(idNekretnine)
                };

                DodatnaOprema? oprema = session.Get<DodatnaOprema>(doID);
                if (oprema != null)
                {
                    oprema.TipOpreme = izmenjenaOprema.TipOpreme;
                    oprema.BesplatnoKoriscenje = izmenjenaOprema.BesplatnoKoriscenje;
                    oprema.CenaKoriscenja = izmenjenaOprema.CenaKoriscenja;

                    session.Update(oprema);
                    session.Flush();
                    return true;
                }
                else
                {
                    return $"Dodatna oprema sa ID {doID} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmene dodatne opreme: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<bool, ErrorMessage> ObrisiDodatnuOpremu(int idOpreme, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina? nekretnina = session.Get<Nekretnina>(idNekretnine);
                if (nekretnina != null)
                {
                    DodatnaOpremaId doID = new DodatnaOpremaId
                    {
                        IdOpreme = idOpreme,
                        Nekretnina = nekretnina
                    };

                    DodatnaOprema? oprema = session.Get<DodatnaOprema>(doID);
                    if (oprema != null)
                    {
                        session.Delete(oprema);
                        session.Flush();
                        return true;
                    }
                    else
                    {
                        return $"Dodatna oprema sa ID {doID} nije pronađena.".ToError(404);
                    }
                }
                else
                {
                    return $"Nekretnina sa ID {idNekretnine} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja dodatne opreme: {ex.Message}.".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Parking

    public async static Task<Result<bool, ErrorMessage>> DodajParkingAsync(ParkingView noviParking, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                ParkingId pID = new()
                {
                    IdParkinga = noviParking.ID.IdParkinga,
                    Nekretnina = nekretnina
                };
                Parking parking = new Parking
                {
                    ID = pID,
                    Besplatan = noviParking.Besplatan,
                    Cena = noviParking.Cena,
                    USastavuNekretnine = noviParking.USastavuNekretnine,
                    USastavuJavnogParkinga = noviParking.USastavuJavnogParkinga
                };
                nekretnina.Parking.Add(parking);

                await session.SaveAsync(parking);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja parkinga: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<ParkingView>, ErrorMessage> VratiSveParkingeNekretnine(int idNekretnine)
    {
        List<ParkingView> parkinzi = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<Parking> sviParkinzi = from parking
                                                   in session.Query<Parking>()
                                                   where parking.ID.Nekretnina.IdNekretnine == idNekretnine
                                                   select parking;

                foreach (Parking p in sviParkinzi)
                {
                    parkinzi.Add(new ParkingView(p));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja parkinga: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return parkinzi;
    }

    public async static Task<Result<ParkingView, ErrorMessage>> VratiParkingAsync(int idParkinga, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                ParkingId pID = new()
                {
                    IdParkinga = idParkinga,
                    Nekretnina = nekretnina
                };
                Parking parking = await session.GetAsync<Parking>(pID);
                if (parking != null)
                {
                    return new ParkingView(parking);
                }
                else
                {
                    return $"Parking sa ID {idParkinga} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja parkinga: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniParkingAsync(ParkingView izmenjeniParking, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                ParkingId pID = new()
                {
                    IdParkinga = izmenjeniParking.ID.IdParkinga,
                    Nekretnina = nekretnina
                };
                Parking parking = await session.GetAsync<Parking>(pID);
                if (parking != null)
                {
                    parking.Besplatan = izmenjeniParking.Besplatan;
                    parking.Cena = izmenjeniParking.Cena;
                    parking.USastavuNekretnine = izmenjeniParking.USastavuNekretnine;
                    parking.USastavuJavnogParkinga = izmenjeniParking.USastavuJavnogParkinga;

                    await session.UpdateAsync(parking);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Parking sa ID {izmenjeniParking.ID.IdParkinga} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene parkinga: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiParkingAsync(int idParkinga, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                ParkingId pID = new()
                {
                    IdParkinga = idParkinga,
                    Nekretnina = nekretnina
                };
                Parking parking = await session.GetAsync<Parking>(pID);
                if (parking != null)
                {
                    await session.DeleteAsync(parking);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Parking sa ID {idParkinga} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja parkinga: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region SajtoviNekretnine

    public async static Task<Result<bool, ErrorMessage>> DodajSajtNekretnineAsync(SajtoviNekretnineView noviSajt, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SajtoviNekretnineId sajtID = new()
                {
                    Sajt = noviSajt.ID.Sajt,
                    Nekretnina = nekretnina
                };
                SajtoviNekretnine sajtIzdavanja = new()
                {
                    ID = sajtID
                };
                nekretnina.SajtoviNekretnine.Add(sajtIzdavanja);

                await session.SaveAsync(sajtIzdavanja);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja sajta nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<SajtoviNekretnineView>, ErrorMessage> VratiSveSajtoveNekretnine(int idNekretnine)
    {
        List<SajtoviNekretnineView> sajtovi = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<SajtoviNekretnine> sviSajtovi = from sajt
                                                            in session.Query<SajtoviNekretnine>()
                                                            where sajt.ID.Nekretnina.IdNekretnine == idNekretnine
                                                            select sajt;

                foreach (SajtoviNekretnine s in sviSajtovi)
                {
                    sajtovi.Add(new SajtoviNekretnineView(s));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja sajtova nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return sajtovi;
    }

    public async static Task<Result<SajtoviNekretnineView, ErrorMessage>> VratiSajtNekretnineAsync(string sajt, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SajtoviNekretnineId sID = new()
                {
                    Sajt = sajt,
                    Nekretnina = nekretnina
                };
                SajtoviNekretnine sajtNekretnine = await session.GetAsync<SajtoviNekretnine>(sID);
                if (sajtNekretnine != null)
                {
                    return new SajtoviNekretnineView(sajtNekretnine);
                }
                else
                {
                    return $"Sajt nekretnine sa sajtom {sajt} i ID-om nekretnine {idNekretnine} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja sajta nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniSajtNekretnineAsync(SajtoviNekretnineView izmenjeniSajt, string stariSajt, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SajtoviNekretnineId sID = new()
                {
                    Sajt = stariSajt,
                    Nekretnina = nekretnina
                };
                SajtoviNekretnine sajtNekretnine = await session.GetAsync<SajtoviNekretnine>(sID);
                SajtoviNekretnineId noviID = new()
                {
                    Sajt = izmenjeniSajt.ID.Sajt,
                    Nekretnina = nekretnina
                };
                if (sajtNekretnine != null)
                {
                    await session.DeleteAsync(sajtNekretnine);
                    await session.FlushAsync();

                    sajtNekretnine = new SajtoviNekretnine { ID = noviID };

                    nekretnina.SajtoviNekretnine.Add(sajtNekretnine);

                    await session.SaveAsync(sajtNekretnine);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Sajt nekretnine sa sajtom {stariSajt} i ID-om nekretnine {idNekretnine} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene sajta nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiSajtNekretnineAsync(string sajt, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SajtoviNekretnineId sID = new()
                {
                    Sajt = sajt,
                    Nekretnina = nekretnina
                };
                SajtoviNekretnine sajtNekretnine = await session.GetAsync<SajtoviNekretnine>(sID);
                if (sajtNekretnine != null)
                {
                    await session.DeleteAsync(sajtNekretnine);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Sajt nekretnine sa sajtom {sajt} i ID-om nekretnine {idNekretnine} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja sajta nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region BrojeviTelefona

    public async static Task<Result<bool, ErrorMessage>> DodajBrojTelefonaAsync(BrojeviTelefonaView noviBroj, string jmbgFizickogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Load<FizickoLice>(jmbgFizickogLica);
                BrojeviTelefona brojTelefona = new BrojeviTelefona
                {
                    BrojTelefona = noviBroj.BrojTelefona,
                    FizickoLice = fizickoLice
                };

                await session.SaveAsync(brojTelefona);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja broja telefona: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<BrojeviTelefonaView>, ErrorMessage> VratiSveBrojeveTelefona(string jmbgFizickogLica)
    {
        List<BrojeviTelefonaView> brojevi = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<BrojeviTelefona> sviBrojevi = from broj
                                                          in session.Query<BrojeviTelefona>()
                                                          where broj.FizickoLice.JMBG == jmbgFizickogLica
                                                          select broj;

                foreach (BrojeviTelefona b in sviBrojevi)
                {
                    brojevi.Add(new BrojeviTelefonaView(b));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja brojeva telefona: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return brojevi;
    }

    public async static Task<Result<BrojeviTelefonaView, ErrorMessage>> VratiBrojTelefonaAsync(string brojTelefona, string jmbgFizickogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Load<FizickoLice>(jmbgFizickogLica);
                BrojeviTelefona brojTelefonaObj = await session.GetAsync<BrojeviTelefona>(brojTelefona);

                if (brojTelefonaObj != null)
                {
                    return new BrojeviTelefonaView(brojTelefonaObj);
                }
                else
                {
                    return $"Broj telefona sa brojem {brojTelefona} i JMBG-om {jmbgFizickogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja broja telefona: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniBrojTelefonaAsync(BrojeviTelefonaView izmenjeniBroj, string jmbgFizickogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Load<FizickoLice>(jmbgFizickogLica);
                BrojeviTelefona brojTelefona = await session.GetAsync<BrojeviTelefona>(izmenjeniBroj.BrojTelefona);

                if (brojTelefona != null)
                {
                    brojTelefona.BrojTelefona = izmenjeniBroj.BrojTelefona;
                    brojTelefona.FizickoLice = fizickoLice;

                    await session.UpdateAsync(brojTelefona);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Broj telefona sa brojem {izmenjeniBroj.BrojTelefona} i JMBG-om {jmbgFizickogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene broja telefona: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiBrojTelefonaAsync(string brojTelefona, string jmbgFizickogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                FizickoLice fizickoLice = session.Load<FizickoLice>(jmbgFizickogLica);
                BrojeviTelefona brojTelefonaObj = await session.GetAsync<BrojeviTelefona>(brojTelefona);
                if (brojTelefonaObj != null)
                {
                    await session.DeleteAsync(brojTelefonaObj);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Broj telefona {brojTelefona} za fizičko lice sa JMBG-om {jmbgFizickogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja broja telefona: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region TelefoniKontaktOsobe

    public async static Task<Result<bool, ErrorMessage>> DodajTelefonKontaktOsobeAsync(TelefoniKontaktOsobeView noviTelefon, string pibPravnogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Load<PravnoLice>(pibPravnogLica);
                TelefoniKontaktOsobe telefon = new TelefoniKontaktOsobe
                {
                    BrojTelefona = noviTelefon.BrojTelefona,
                    PravnoLice = pravnoLice
                };

                await session.SaveAsync(telefon);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja telefona kontakt osobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<TelefoniKontaktOsobeView>, ErrorMessage> VratiSveTelefoneKontaktOsobe(string pibPravnogLica)
    {
        List<TelefoniKontaktOsobeView> telefoni = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<TelefoniKontaktOsobe> sviTelefoni = from telefon
                                                                in session.Query<TelefoniKontaktOsobe>()
                                                                where telefon.PravnoLice.PIB == pibPravnogLica
                                                                select telefon;

                foreach (TelefoniKontaktOsobe t in sviTelefoni)
                {
                    telefoni.Add(new TelefoniKontaktOsobeView(t));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja telefona kontakt osobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return telefoni;
    }

    public async static Task<Result<TelefoniKontaktOsobeView, ErrorMessage>> VratiTelefonKontaktOsobeAsync(string brojTelefona, string pibPravnogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Load<PravnoLice>(pibPravnogLica);
                TelefoniKontaktOsobe telefon = await session.GetAsync<TelefoniKontaktOsobe>(brojTelefona);
                if (telefon != null)
                {
                    return new TelefoniKontaktOsobeView(telefon);
                }
                else
                {
                    return $"Telefon kontakt osobe sa brojem {brojTelefona} i PIB-om {pibPravnogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja telefona kontakt osobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniTelefonKontaktOsobeAsync(TelefoniKontaktOsobeView izmenjeniTelefon, string pibPravnogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Load<PravnoLice>(pibPravnogLica);
                TelefoniKontaktOsobe telefon = await session.GetAsync<TelefoniKontaktOsobe>(izmenjeniTelefon.BrojTelefona);
                if (telefon != null)
                {
                    telefon.BrojTelefona = izmenjeniTelefon.BrojTelefona;
                    telefon.PravnoLice = pravnoLice;

                    await session.UpdateAsync(telefon);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Telefon kontakt osobe sa brojem {izmenjeniTelefon.BrojTelefona} i PIB-om {pibPravnogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene telefona kontakt osobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiTelefonKontaktOsobeAsync(string brojTelefona, string pibPravnogLica)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                PravnoLice pravnoLice = session.Load<PravnoLice>(pibPravnogLica);
                TelefoniKontaktOsobe telefon = await session.GetAsync<TelefoniKontaktOsobe>(brojTelefona);
                if (telefon != null)
                {
                    await session.DeleteAsync(telefon);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Telefon kontakt osobe {brojTelefona} za pravno lice sa PIB-om {pibPravnogLica} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja telefona kontakt osobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Soba

    public async static Task<Result<bool, ErrorMessage>> DodajSobuAsync(SobaView novaSoba, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SobaId sID = new()
                {
                    IdSobe = novaSoba.ID.IdSobe,
                    Nekretnina = nekretnina
                };
                Soba soba = new()
                {
                    ID = sID
                };
                nekretnina.Sobe.Add(soba);

                await session.SaveAsync(soba);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<SobaView>, ErrorMessage> VratiSveSobe()
    {
        List<SobaView> sobe = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<Soba> sveSobe = from soba in session.Query<Soba>() select soba;

                foreach (Soba s in sveSobe)
                {
                    sobe.Add(new SobaView(s));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih soba: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return sobe;
    }

    public static Result<List<SobaView>, ErrorMessage> VratiSveSobeNekretnine(int idNekretnine)
    {
        List<SobaView> sobe = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<Soba> sveSobe = from soba
                                            in session.Query<Soba>()
                                            where soba.ID.Nekretnina.IdNekretnine == idNekretnine
                                            select soba;

                foreach (Soba s in sveSobe)
                {
                    sobe.Add(new SobaView(s));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja soba nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return sobe;
    }

    public async static Task<Result<SobaView, ErrorMessage>> VratiSobuAsync(int idSobe, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = await session.GetAsync<Nekretnina>(idNekretnine);
                if (nekretnina != null)
                {
                    SobaId sID = new()
                    {
                        IdSobe = idSobe,
                        Nekretnina = nekretnina
                    };
                    Soba soba = await session.GetAsync<Soba>(sID);
                    if (soba != null)
                    {
                        return new SobaView(soba);
                    }
                    else
                    {
                        return $"Soba sa ID {idSobe} nije pronađena.".ToError(404);
                    }
                }
                else
                {
                    return $"Nekretnina sa ID {idNekretnine} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiSobuAsync(int idSobe, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = session.Load<Nekretnina>(idNekretnine);
                SobaId sID = new()
                {
                    IdSobe = idSobe,
                    Nekretnina = nekretnina
                };
                Soba soba = await session.GetAsync<Soba>(sID);
                if (soba != null)
                {
                    await session.DeleteAsync(soba);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Soba sa ID {idSobe} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region ZajednickeProstorije

    public async static Task<Result<bool, ErrorMessage>> DodajZajednickuProstorijuAsync(ZajednickeProstorijeView novaProstorija, int idSobe, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Soba soba = await session.LoadAsync<Soba>(new SobaId { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) });
                ZajednickeProstorijeId zpID = new ZajednickeProstorijeId
                {
                    Soba = soba,
                    ZajednickaProstorija = novaProstorija.ID.ZajednickaProstorija
                };
                ZajednickeProstorije zajednickaProstorija = new ZajednickeProstorije
                {
                    ID = zpID
                };

                await session.SaveAsync(zajednickaProstorija);
                await session.FlushAsync();
                return true;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja zajedničke prostorije: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public static Result<List<ZajednickeProstorijeView>, ErrorMessage> VratiSveZajednickeProstorijeSobe(int idSobe, int idNekretnine)
    {
        List<ZajednickeProstorijeView> prostorije = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IEnumerable<ZajednickeProstorije> sveProstorije = from zp
                                                                  in session.Query<ZajednickeProstorije>()
                                                                  where zp.ID.Soba.ID.IdSobe == idSobe && zp.ID.Soba.ID.Nekretnina.IdNekretnine == idNekretnine
                                                                  select zp;

                foreach (ZajednickeProstorije zp in sveProstorije)
                {
                    prostorije.Add(new ZajednickeProstorijeView(zp));
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih zajedničkih prostorija: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return prostorije;
    }

    public static Result<List<ZajednickeProstorijeView>, ErrorMessage> VratiSveZajednickeProstorijeNajma(int idNajma)
    {
        List<ZajednickeProstorijeView> prostorije = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var sveProstorije = session.Query<ZajednickeProstorije>()
                                    .Where(zp => session.Query<IznajmljenaSoba>()
                                         .Any(iznajmljena => iznajmljena.ID.Najam.IdNajma == idNajma &&
                                           iznajmljena.ID.Soba.ID.Nekretnina.IdNekretnine == zp.ID.Soba.ID.Nekretnina.IdNekretnine &&
                                           iznajmljena.ID.Soba.ID.IdSobe == zp.ID.Soba.ID.IdSobe))
                                     .Select(zp => new ZajednickeProstorijeView(zp))
                                     .Distinct().ToList();

                prostorije.AddRange(sveProstorije);
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja zajedničkih prostorija najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }

        return prostorije;
    }

    public async static Task<Result<ZajednickeProstorijeView, ErrorMessage>> VratiZajednickuProstorijuAsync(int idSobe, int idNekretnine, string zajednickaProstorija)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Soba soba = await session.LoadAsync<Soba>(new SobaId { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) });
                ZajednickeProstorijeId zpID = new ZajednickeProstorijeId
                {
                    Soba = soba,
                    ZajednickaProstorija = zajednickaProstorija
                };
                ZajednickeProstorije prostorija = await session.GetAsync<ZajednickeProstorije>(zpID);
                if (prostorija != null)
                {
                    return new ZajednickeProstorijeView(prostorija);
                }
                else
                {
                    return $"Zajednička prostorija sa ID {zpID} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja zajedničke prostorije: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniZajednickuProstorijuAsync(ZajednickeProstorijeView izmenjenaProstorija, int idSobe, int idNekretnine)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Soba soba = await session.LoadAsync<Soba>(new SobaId { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) });
                ZajednickeProstorijeId zpID = new ZajednickeProstorijeId
                {
                    Soba = soba,
                    ZajednickaProstorija = izmenjenaProstorija.ID.ZajednickaProstorija
                };
                ZajednickeProstorije prostorija = await session.GetAsync<ZajednickeProstorije>(zpID);
                if (prostorija != null)
                {
                    prostorija.ID = zpID;

                    await session.UpdateAsync(prostorija);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Zajednička prostorija sa ID {zpID} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene zajedničke prostorije: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiZajednickuProstorijuAsync(int idSobe, int idNekretnine, string zajednickaProstorija)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Soba soba = await session.LoadAsync<Soba>(new SobaId { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) });
                ZajednickeProstorijeId zpID = new ZajednickeProstorijeId
                {
                    Soba = soba,
                    ZajednickaProstorija = zajednickaProstorija
                };
                ZajednickeProstorije prostorija = await session.GetAsync<ZajednickeProstorije>(zpID);
                if (prostorija != null)
                {
                    await session.DeleteAsync(prostorija);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Zajednička prostorija sa ID {zpID} nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja zajedničke prostorije: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Close();
            session?.Dispose();
        }
    }

    #endregion

    #region Najam

    public async static Task<Result<Najam, ErrorMessage>> KreirajNajamAsync(NajamView noviNajam, int idNekretnine, string mbrAgenta, int? idSpoljnogSaradnika = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Nekretnina nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine);
                Agent agent = await session.LoadAsync<Agent>(mbrAgenta);
                SpoljniSaradnik? spoljni = null;

                if (idSpoljnogSaradnika.HasValue)
                {
                    SpoljniSaradnikId ssID = new()
                    {
                        AgentAngazovanja = agent,
                        IdSaradnika = idSpoljnogSaradnika.Value
                    };
                    spoljni = await session.LoadAsync<SpoljniSaradnik>(ssID);
                }

                double zaradaOdDodatnihUsluga = 0;
                foreach (var dop in nekretnina.DodatnaOprema)
                {
                    zaradaOdDodatnihUsluga += dop.CenaKoriscenja ?? 0;
                }
                foreach (var p in nekretnina.Parking)
                {
                    zaradaOdDodatnihUsluga += p.Cena ?? 0;
                }
                if (noviNajam.Popust >= 99 || noviNajam.Popust <= 1)
                {
                    return "Loš popust, mora biti između 1 i 99".ToError(400);
                }

                Najam najam = new Najam
                {
                    DatumPocetka = noviNajam.DatumPocetka,
                    DatumZavrsetka = noviNajam.DatumZavrsetka,
                    BrojDana = (noviNajam.DatumZavrsetka - noviNajam.DatumPocetka).Days,
                    CenaPoDanu = noviNajam.CenaPoDanu,
                    Popust = noviNajam.Popust > 0 ? noviNajam.Popust : null,
                    CenaSaPopustom = noviNajam.Popust > 0 ? noviNajam.CenaPoDanu - (noviNajam.CenaPoDanu / (100 / noviNajam.Popust)) : null,
                    ZaradaOdDodatnihUsluga = zaradaOdDodatnihUsluga,
                    // Ovo da se proveri!
                    UkupnaCena = (noviNajam.Popust > 0 ? noviNajam.CenaSaPopustom * noviNajam.BrojDana + zaradaOdDodatnihUsluga : noviNajam.CenaPoDanu * noviNajam.BrojDana + zaradaOdDodatnihUsluga) ?? noviNajam.CenaPoDanu * noviNajam.BrojDana + zaradaOdDodatnihUsluga,
                    ProvizijaAgencije = noviNajam.ProvizijaAgencije,
                    Nekretnina = nekretnina,
                    Agent = agent,
                    SpoljniSaradnik = spoljni
                };

                agent.RealizovaniNajmovi.Add(najam);
                nekretnina.Najmovi.Add(najam);
                spoljni?.RealizovaniNajmovi.Add(najam);

                return najam;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom kreiranja najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> DodajNajamAsync(NajamView noviNajam, int idNekretnine, string mbrAgenta, int? idSpoljnogSaradnika = null)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var result = await KreirajNajamAsync(noviNajam, idNekretnine, mbrAgenta, idSpoljnogSaradnika);

                if (result.IsSuccess)
                {
                    await session.SaveAsync(result.Data);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return result.Error;
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<List<NajamView>, ErrorMessage>> VratiSveNajmoveAsync()
    {
        List<NajamView> najmovi = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var sviNajmovi = await session.Query<Najam>().ToListAsync();

                foreach (Najam n in sviNajmovi)
                {
                    najmovi.Add(new NajamView(n));
                }

                return najmovi;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih najmova: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<List<NajamView>, ErrorMessage>> VratiSveNajmoveNekretnineAsync(int idNekretnine)
    {
        List<NajamView> najmovi = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var sviNajmovi = await session.Query<Najam>()
                                              .Where(n => n.Nekretnina.IdNekretnine == idNekretnine)
                                              .ToListAsync();

                foreach (Najam n in sviNajmovi)
                {
                    najmovi.Add(new NajamView(n));
                }

                return najmovi;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja najmova nekretnine: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<NajamView, ErrorMessage>> VratiNajamAsync(int idNajma)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Najam najam = await session.GetAsync<Najam>(idNajma);
                if (najam != null)
                {
                    return new NajamView(najam);
                }
                else
                {
                    return $"Najam sa ID {idNajma} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> IzmeniNajamAsync(int idNajma, NajamView izmenjenNajam)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Najam najam = await session.GetAsync<Najam>(idNajma);
                if (najam != null)
                {
                    najam.DatumPocetka = izmenjenNajam.DatumPocetka;
                    najam.DatumZavrsetka = izmenjenNajam.DatumZavrsetka;
                    najam.BrojDana = (izmenjenNajam.DatumZavrsetka - izmenjenNajam.DatumPocetka).Days;
                    najam.CenaPoDanu = izmenjenNajam.CenaPoDanu;
                    najam.Popust = izmenjenNajam.Popust;
                    najam.CenaSaPopustom = izmenjenNajam.CenaPoDanu * izmenjenNajam.BrojDana * (1 - izmenjenNajam.Popust / 100.0);
                    najam.ZaradaOdDodatnihUsluga = izmenjenNajam.ZaradaOdDodatnihUsluga;
                    najam.UkupnaCena = (izmenjenNajam.Popust > 0 ? izmenjenNajam.CenaSaPopustom * izmenjenNajam.BrojDana + izmenjenNajam.ZaradaOdDodatnihUsluga : izmenjenNajam.CenaPoDanu * izmenjenNajam.BrojDana + izmenjenNajam.ZaradaOdDodatnihUsluga) ?? izmenjenNajam.CenaPoDanu * izmenjenNajam.BrojDana + izmenjenNajam.ZaradaOdDodatnihUsluga;
                    najam.ProvizijaAgencije = izmenjenNajam.ProvizijaAgencije;

                    await session.UpdateAsync(najam);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Najam sa ID {izmenjenNajam.IdNajma} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom izmjene najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiNajamAsync(int idNajma)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                Najam najam = await session.GetAsync<Najam>(idNajma);
                if (najam != null)
                {
                    await session.DeleteAsync(najam);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return $"Najam sa ID {idNajma} nije pronađen.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja najma: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    #endregion

    #region IznajmljenaSoba

    public async static Task<Result<bool, ErrorMessage>> DodajIznajmljenuSobuAsync(IznajmljenaSobaView novaSoba, int idNekretnine, List<int> idSoba, string mbrAgenta, int? idSpoljnog)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var result = await KreirajNajamAsync(novaSoba.ID.Najam, idNekretnine, mbrAgenta, idSpoljnog);

                if (result.IsSuccess)
                {
                    var najam = result.Data;
                    List<Soba> sobe = new();
                    List<IznajmljenaSoba> iznajmljeneSobe = new();

                    foreach (int idS in idSoba)
                    {
                        Soba soba = await session.LoadAsync<Soba>(new SobaId { IdSobe = idS, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) });
                        sobe.Add(soba);

                        IznajmljenaSobaId iznID = new()
                        {
                            Soba = soba,
                            Najam = najam
                        };

                        iznajmljeneSobe.Add(new() { ID = iznID });
                    }

                    najam.IznajmljivanjaSoba = iznajmljeneSobe;

                    await session.SaveOrUpdateAsync(najam);
                    await session.FlushAsync();
                    return true;
                }
                else
                {
                    return result.Error;
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom dodavanja iznajmljene sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<List<IznajmljenaSobaView>, ErrorMessage>> VratiSveIznajmljeneSobeAsync()
    {
        List<IznajmljenaSobaView> iznajmljeneSobe = new();
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                var sveSobe = await session.Query<IznajmljenaSoba>().ToListAsync();

                foreach (IznajmljenaSoba s in sveSobe)
                {
                    iznajmljeneSobe.Add(new IznajmljenaSobaView(s));
                }

                return iznajmljeneSobe;
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja svih iznajmljenih soba: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<IznajmljenaSobaView?, ErrorMessage>> VratiIznajmljenuSobuAsync(int idSobe, int idNekretnine, int idNajma)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                SobaId sID = new() { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) };
                IznajmljenaSobaId id = new IznajmljenaSobaId
                {
                    Soba = new Soba { ID = sID },
                    Najam = await session.LoadAsync<Najam>(idNajma)
                };

                IznajmljenaSoba iznajmljenaSoba = await session.GetAsync<IznajmljenaSoba>(id);
                if (iznajmljenaSoba != null)
                {
                    return new IznajmljenaSobaView(iznajmljenaSoba);
                }
                else
                {
                    return $"Iznajmljena soba nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom vraćanja iznajmljene sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    public async static Task<Result<bool, ErrorMessage>> ObrisiIznajmljenuSobuAsync(int idSobe, int idNekretnine, int idNajma)
    {
        ISession? session = null;
        try
        {
            session = DataLayer.GetSession();
            if (session != null && session.IsOpen)
            {
                IznajmljenaSobaId id = new()
                {
                    Soba = new Soba { ID = new() { IdSobe = idSobe, Nekretnina = await session.LoadAsync<Nekretnina>(idNekretnine) } },
                    Najam = await session.LoadAsync<Najam>(idNajma)
                };

                IznajmljenaSoba iznajmljenaSoba = await session.GetAsync<IznajmljenaSoba>(id);
                if (iznajmljenaSoba != null)
                {
                    Najam najam = iznajmljenaSoba.ID.Najam;
                    await session.DeleteAsync(iznajmljenaSoba);
                    await session.FlushAsync();
                    if (najam.IznajmljivanjaSoba.Count == 0)
                    {
                        await session.DeleteAsync(najam);
                        await session.FlushAsync();
                    }
                    return true;
                }
                else
                {
                    return "Iznajmljena soba nije pronađena.".ToError(404);
                }
            }
            else
            {
                return "Nemoguće otvoriti sesiju.".ToError(403);
            }
        }
        catch (Exception ex)
        {
            return $"Greška prilikom brisanja iznajmljene sobe: {ex.Message}".ToError(400);
        }
        finally
        {
            session?.Dispose();
        }
    }

    #endregion
}