using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko} | {s.Miasto}");
    }

    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }

    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko}");
    }

    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var przedmiot = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics");

        if (przedmiot == null)
            return new[] { "Brak przedmiotu z kategorii Analytics" };

        return new[]
        {
            $"{przedmiot.Nazwa} | {przedmiot.DataStartu}"
        };
    }

    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        bool istnieje = DaneUczelni.Zapisy
            .Any(z => !z.CzyAktywny);

        return new[] { $"Czy istnieje nieaktywny zapis: {istnieje}" };
    }

    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool wszyscy = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrEmpty(p.Katedra));

        return new[] { $"Czy wszyscy mają katedrę: {wszyscy}" };
    }

    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        int liczba = DaneUczelni.Zapisy
            .Count(z => z.CzyAktywny);

        return new[] { $"Liczba aktywnych zapisów: {liczba}" };
    }

    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);
    }

    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu} | StudentId: {z.StudentId} | PrzedmiotId: {z.PrzedmiotId}");
    }

    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} | {p.Kategoria}");
    }

    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (s, z) => $"{s.Imie} {s.Nazwisko} | {z.DataZapisu}"
            );
    }

    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .SelectMany(z =>
                DaneUczelni.Studenci.Where(s => s.Id == z.StudentId),
                (z, s) => new { z, s })
            .SelectMany(temp =>
                DaneUczelni.Przedmioty.Where(p => p.Id == temp.z.PrzedmiotId),
                (temp, p) => $"{temp.s.Imie} {temp.s.Nazwisko} | {p.Nazwa}"
            );
    }

    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => p.Nazwa
            )
            .GroupBy(nazwa => nazwa)
            .Select(g => $"{g.Key} | {g.Count()}");
    }

    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa != null)
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (z, p) => new { p.Nazwa, z.OcenaKoncowa }
            )
            .GroupBy(x => x.Nazwa)
            .Select(g => $"{g.Key} | {g.Average(x => x.OcenaKoncowa)}");
    }

    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) => $"{pr.Imie} {pr.Nazwisko} | {przedmioty.Count()}"
            );
    }

    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa != null)
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => new { s.Imie, s.Nazwisko, z.OcenaKoncowa }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | {g.Max(x => x.OcenaKoncowa)}");
    }

    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => new { s.Imie, s.Nazwisko }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | {g.Count()}");
    }

    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .Join(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (p, z) => new { p.Nazwa, z.OcenaKoncowa }
            )
            .GroupBy(x => x.Nazwa)
            .Where(g => g.All(x => x.OcenaKoncowa == null))
            .Select(g => g.Key);
    }

    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                pr => pr.Id,
                p => p.ProwadzacyId,
                (pr, przedmioty) => new { pr, przedmioty }
            )
            .SelectMany(
                x => x.przedmioty.DefaultIfEmpty(),
                (x, p) => new { x.pr, p }
            )
            .SelectMany(
                x => DaneUczelni.Zapisy
                    .Where(z => x.p != null && z.PrzedmiotId == x.p.Id && z.OcenaKoncowa != null)
                    .DefaultIfEmpty(),
                (x, z) => new { x.pr, z?.OcenaKoncowa }
            )
            .Where(x => x.OcenaKoncowa != null)
            .GroupBy(x => new { x.pr.Imie, x.pr.Nazwisko })
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | {g.Average(x => x.OcenaKoncowa)}");
    }

    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Join(
                DaneUczelni.Studenci,
                z => z.StudentId,
                s => s.Id,
                (z, s) => s.Miasto
            )
            .GroupBy(miasto => miasto)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} | {g.Count()}");
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}