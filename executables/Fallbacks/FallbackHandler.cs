namespace Executables.Fallbacks;

public interface IFallbackHandler<in T1, in TException, out T2> where TException : Exception {

  T2 Fallback(T1 input, TException e);

}