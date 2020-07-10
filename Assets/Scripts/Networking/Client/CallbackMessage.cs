using System;
using UnityEngine;
using Utils;

namespace Networking.Client {
  [Serializable]
  public class CallbackMessages {
    public CallbackMessage callbackMessages {
      get;
      set;
    }
  }

  [Serializable]
  public class CallbackMessage {
    public string message {
      get;
      set;
    }
    = null;
    public CallbackIdentity identity {
      get;
      set;
    }
    = null;
    public string room {
      get;
      set;
    }
    = null;
    public int[] maps {
      get;
      set;
    }
    = null;
  }

  [Serializable]
  public class CallbackIdentity {
    public int classes {
      get;
      set;
    }
    = -1;
    public int weapon {
      get;
      set;
    }
    = -1;
  }
}